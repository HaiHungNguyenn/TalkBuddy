﻿using Microsoft.AspNetCore.SignalR;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;
using TalkBuddy.Domain.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TalkBuddy.Presentation.SignalR
{
    public class ChatHub : Hub
    {
        public readonly static List<UserConnection> _ConnectionRooms = new List<UserConnection>();
        private readonly static List<UserConnection> _ConnectionPresences = new List<UserConnection>();
        private readonly IMessageService _messageService;
        private readonly IClientChatBoxService _clientChatBoxService;
        private readonly IClientService _clientService;
        private readonly IChatBoxService _chatBoxService;
        public ChatHub(IChatBoxService chatBoxService,
            IMessageService messageService,
            IClientChatBoxService clientChatBoxService,
            IClientService clientService)
        {
            _chatBoxService = chatBoxService;
            _messageService = messageService;
            _clientChatBoxService = clientChatBoxService;
            _clientService = clientService;
        }

        public override async Task OnConnectedAsync()
        {

            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Session.GetString(SessionConstants.USER_ID);
            List<ClientChatBoxDto> returnList = new List<ClientChatBoxDto>();
            if (!string.IsNullOrEmpty(userId))
            {
                //replace clientChatBoxes by clientChatBoxesContainMessages if want to load only chatBox which already have conversation - means messages != null
                // var clientChatBoxesContainMessages = await _clientChatBoxService.GetClientChatBoxesIncludeNotEmptyMessages(new Guid(userId));
                var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(new Guid(userId));
                await Clients.Caller.SendAsync("InitializeChat", await GetClientChatBox(userId, clientChatBoxes));
                if (!_ConnectionPresences.Any(x => x.UserId.Equals(userId) && x.ConnectionId.Equals(Context.ConnectionId.ToString())))
                {
                    _ConnectionPresences.Add(
                        new UserConnection
                        {
                            UserId = userId,
                            ConnectionId = Context.ConnectionId
                        });
                }
            }

            await base.OnConnectedAsync();
        }



        public override Task OnDisconnectedAsync(Exception ex)
        {
            try
            {
                var httpContext = Context.GetHttpContext();

                var userId = httpContext.Session.GetString(SessionConstants.USER_ID);
                var user = _ConnectionPresences.Where(u => u.UserId.Equals(userId) && u.ConnectionId.Equals(Context.ConnectionId.ToString())).First();
                _ConnectionPresences.Remove(user);
                foreach (var room in _ConnectionRooms.Where(x => x.UserId.Equals(userId) && x.ConnectionId.Equals(httpContext.Connection.Id.ToString())))
                {
                    _ConnectionRooms.Remove(room);
                }

            }
            catch (Exception exep)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + exep.Message);
            }

            return base.OnDisconnectedAsync(ex);
        }

        public async Task<IList<MessageDto>> GetMessages(Guid chatBoxId)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Session.GetString(SessionConstants.USER_ID);

            var messageReturns = new List<MessageDto>();
            if (_ConnectionPresences.Any(x => x.UserId.Equals(userId)))
            {
                List<UserConnection> userInChatList = _ConnectionRooms.Where(x => x.UserId.Equals(userId)
                                                && !x.ChatBoxId.Equals(chatBoxId)
                                                && x.ConnectionId.Equals(Context.ConnectionId)).ToList();
                if (userInChatList != null && userInChatList.Any())
                {
                    foreach (var room in userInChatList)
                    {
                        _ConnectionRooms.Remove(room);
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ChatBoxId);
                    }

                }
                _ConnectionRooms.Add(new UserConnection
                {
                    UserId = userId,
                    ChatBoxId = chatBoxId.ToString(),
                    ConnectionId = Context.ConnectionId
                });
                await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId.ToString());

                var messages = await _messageService.GetMessages(chatBoxId);

                foreach (var message in messages)
                {
                    var mess = new MessageDto
                    {
                        Medias = message.Medias,
                        SenderName = message.Sender.Name,
                        SenderAvatar = message.Sender.ProfilePicture,
                        Content = message.Content,
                        SenderId = message.SenderId,
                        ChatBoxId = chatBoxId,
                        SentDate = message.SentDate,
                        MessageType = message.MessageType.ToString(),
                        IsYourOwnMess = userId.Equals(message.SenderId.ToString())

                    };
                    messageReturns.Add(mess);
                }

            }
            return messageReturns;
        }

        public async Task SearchByChatBoxName(string searchString)
        {
            var userId = Context.GetHttpContext().Session.GetString(SessionConstants.USER_ID);
            if (string.IsNullOrEmpty(searchString))
            {
                var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(new Guid(userId));
                await Clients.Caller.SendAsync("InitializeChat", await GetClientChatBox(userId, clientChatBoxes));
                return;
            }
            var chatBox = await _clientChatBoxService
                .GetClientOfChatBoxesOfAUserBySearchName(searchString, new Guid(userId));
            if (chatBox != null)
            {
                await Clients.Caller.SendAsync("InitializeChat", await GetClientChatBox(userId, chatBox));
            }
            else
            {
                //await Clients.Caller.SendAsync("ChatBoxNotExist");
            }

        }

        public async Task SendMessage(string chatBoxId, string message)
        {
            var httpContext = Context.GetHttpContext();
            var fromUserId = httpContext.Session.GetString(SessionConstants.USER_ID);
            var sender = await _clientService.GetClientById(new Guid(fromUserId));
            Message messageObject = new Message
            {
                Content = message,
                SentDate = DateTime.Now,
                ChatBoxId = new Guid(chatBoxId),
                SenderId = new Guid(fromUserId)

            };
            //uncomment this if want to implement rule: if have no messages before do not present chatbox
            //var IsChatBoxContainMessages = _chatBoxService.GetChatBoxAsync(new Guid(chatBoxId)).Result.Messages.Any();

            //if (!IsChatBoxContainMessages)
            //{
            //    await Clients.Caller.SendAsync("InitializeChat", await GetClientChatBox(fromUserId));
            //}
            await _messageService.AddMessage(messageObject);
            //[Nhi]3/4/2024: fix message return type from string to object
            var messageReturnForSender = new MessageDto
            {
                Medias = messageObject.Medias,
                SenderName = messageObject.Sender.Name,
                SenderAvatar = messageObject.Sender.ProfilePicture,
                Content = messageObject.Content,
                SenderId = messageObject.SenderId,
                SentDate = messageObject.SentDate,
                IsYourOwnMess = true,
                MessageType = messageObject.MessageType.ToString()

            };

            var messReturnForOthers = new MessageDto
            {
                Medias = messageObject.Medias,
                SenderName = messageObject.Sender.Name,
                SenderAvatar = messageObject.Sender.ProfilePicture,
                Content = messageObject.Content,
                SenderId = messageObject.SenderId,
                SentDate = messageObject.SentDate,
                IsYourOwnMess = false,
                MessageType = messageObject.MessageType.ToString()
            };

            //var currentUserConnection = _ConnectionRooms.Where(x => x.UserId.Equals(fromUserId) && x.ChatBoxId.Equals(chatBoxId)).ToList();
            //foreach (var connection in currentUserConnection)
            //{
            //    await Clients.Client(connection.ConnectionId).SendAsync("ReceiveMessage", sender.Name, messageReturnForSender);
            //}
            //await Clients.GroupExcept(chatBoxId, currentUserConnection
            //    .Select(x => x.ConnectionId)).SendAsync("ReceiveMessage", sender.Name, messReturnForOthers);
            var allClientInChat = await _clientChatBoxService.GetClientOfChatBoxes(new Guid(chatBoxId));
            foreach (var client in allClientInChat)
            {
                if (_ConnectionPresences.Any(x => x.UserId.Equals(client.ClientId.ToString())))
                {
                    var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(client.ClientId);
                    var firstOrDefaultclientChatBoxes = clientChatBoxes.FirstOrDefault();
                    if (firstOrDefaultclientChatBoxes != null)
                    {
                        var currentUserConnectionOfCurrentUser = _ConnectionPresences.Where(x => x.UserId.Equals(client.ClientId.ToString())).Select(x => x.ConnectionId).ToList();
                        foreach (var connection in currentUserConnectionOfCurrentUser)
                        {
                            await Clients.Client(connection).SendAsync("InitializeChatOrder", await GetClientChatBox(client.ClientId.ToString(), clientChatBoxes));
                            var chatBoxUpdate = await GetClientChatBox(client.ClientId.ToString(),
                                await _clientChatBoxService.GetClientOfChatBoxes(new Guid(chatBoxId), client.ClientId));
                            await Clients.Client(connection).SendAsync("UpdateMessagesToTop",
                                              chatBoxUpdate.FirstOrDefault());
                        }

                    }

                }
            }

            //[Nhi]3/4/2024: fix message return type from string to object
        }

        public async Task ExitGroupChat(string chatBoxId)
        {
            var userId = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_ID);
            var userName = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_NAME);
            await _clientChatBoxService.RemoveClientFromChatBox(new Guid(chatBoxId), new Guid(userId));
            var chatBox = await _chatBoxService.GetChatBoxAsync(new Guid(chatBoxId));
            var notiMess = new Message
            {
                Content = $"{userName} left the group",
                SentDate = DateTime.Now,
                SenderId = new Guid(userId),
                MessageType = Domain.Enums.MessageTypes.Notification
            };
            chatBox.Messages.Add(notiMess);
            var messReturnForOthers = new MessageDto
            {
                Content = notiMess.Content,
                SenderId = notiMess.SenderId,
                SentDate = notiMess.SentDate,
                IsYourOwnMess = true,
                MessageType = notiMess.MessageType.ToString()
            };
            await _chatBoxService.UpdateChatBox(chatBox);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatBoxId);
            await Clients.Groups(chatBoxId).SendAsync("ReceiveMessage", userName, messReturnForOthers);
            //update group chat list
            //////////////
            var currentUserConnection = _ConnectionRooms.Where(x => x.UserId.Equals(userId) && x.ChatBoxId.Equals(chatBoxId)).ToList();
            var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(new Guid(userId));

            foreach (var connection in currentUserConnection)
            {
                await Clients.Client(connection.ConnectionId).SendAsync("InitializeChat", await GetClientChatBox(userId, clientChatBoxes));
            }
        }

        public async Task RemoveClientFromChatBox(string clientId, string chatBoxId)
        {
            var userId = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_ID);
            var userName = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_NAME);
            var client = await _clientService.GetClientById(new Guid(clientId));
            await _clientChatBoxService.RemoveClientFromChatBox(new Guid(chatBoxId), new Guid(clientId));
            var chatBox = await _chatBoxService.GetChatBoxAsync(new Guid(chatBoxId));
            var notiMess = new Message
            {
                Content = $"{userName} removed {client.Name} from the group",
                SentDate = DateTime.Now,
                SenderId = new Guid(userId),
                MessageType = Domain.Enums.MessageTypes.Notification
            };
            chatBox.Messages.Add(notiMess);
            var messReturnForOthers = new MessageDto
            {
                Content = notiMess.Content,
                SenderId = notiMess.SenderId,
                SentDate = notiMess.SentDate,
                IsYourOwnMess = true,
                MessageType = notiMess.MessageType.ToString()
            };
            await _chatBoxService.UpdateChatBox(chatBox);
            await Clients.Groups(chatBoxId).SendAsync("ReceiveMessage", userName, messReturnForOthers);

            var clientChatBoxesUpdateModal = await _clientChatBoxService.GetClientOfChatBoxes(new Guid(chatBoxId));
            var clients = clientChatBoxesUpdateModal.Select(c => c.Client);            
            var clientChatBoxUpdateModal = await _clientChatBoxService.GetClientOfChatBoxes(new Guid(chatBoxId), new Guid(userId));
            await Clients.Caller.SendAsync("ShowClientsOfChatBox", clients, chatBoxId, clientChatBoxUpdateModal.FirstOrDefault()?.IsModerator);

            //update group chat list
            //////////////
            var currentUserConnection = _ConnectionRooms.Where(x => x.UserId.Equals(clientId) && x.ChatBoxId.Equals(chatBoxId)).ToList();

            var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(new Guid(clientId));

            foreach (var connection in currentUserConnection)
            {
                await Clients.Client(connection.ConnectionId).SendAsync("InitializeChat", await GetClientChatBox(clientId, clientChatBoxes));

            }

        }

        public async Task GetFriendsListNotInChat(string chatBoxId)
        {
            var userId = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_ID);
            var friends = await _clientChatBoxService.GetFriendsNotInChatBoxes(new Guid(chatBoxId), new Guid(userId));
            await Clients.Caller.SendAsync("showFriendsListModal", friends, chatBoxId);
        }
        public async Task ChangeChatBoxName(string chatBoxId, string changeName)
        {
            var userId = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_ID);
            var chatBox = await _chatBoxService.GetChatBoxAsync(new Guid(chatBoxId));
            chatBox.ChatBoxName = changeName;
            var mess = new Message
            {
                Content = $"{Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_NAME)} changed group name to {changeName}",
                SentDate = DateTime.Now,
                SenderId = new Guid(userId),
                MessageType = Domain.Enums.MessageTypes.Notification
            };
            var messToReturn = new MessageDto
            {
                Content = mess.Content,
                SenderId = mess.SenderId,
                SentDate = mess.SentDate,
                IsYourOwnMess = false,
                MessageType = mess.MessageType.ToString()
            };
            chatBox.Messages.Add(mess);
            await _chatBoxService.UpdateChatBox(chatBox);

            var clientInChat = await _clientChatBoxService.GetClientOfChatBoxes(new Guid(chatBoxId));
            foreach (var client in clientInChat)
            {
                if (_ConnectionPresences.Any(x => x.UserId.Equals(client.ClientId.ToString())))
                {
                    var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(client.ClientId);
                    ///
                    var currentUserConnection = _ConnectionPresences.Where(x => x.UserId.Equals(client.ClientId.ToString())).Select(x => x.ConnectionId).ToList();
                    foreach (var connection in currentUserConnection)
                    {
                        await Clients.Client(connection).SendAsync("InitializeChat", await GetClientChatBox(client.ClientId.ToString(), clientChatBoxes));
                        var chatBoxUpdate = await GetClientChatBox(client.ClientId.ToString(),
                            await _clientChatBoxService.GetClientOfChatBoxes(new Guid(chatBoxId), client.ClientId));
                        await Clients.Client(connection).SendAsync("UpdateMessagesForChatBox",
                    chatBoxUpdate.FirstOrDefault());
                    }
                    ///
                }
            }

            await Clients.Group(chatBoxId).SendAsync("ReceiveMessage", Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_NAME), messToReturn);

        }
        public async Task AddPeopleToChatBox(List<string> addPeopleList, string chatBoxId)
        {
            var chatBox = await _chatBoxService.GetChatBoxAsync(new Guid(chatBoxId));

            foreach (var people in addPeopleList)
            {
                var person = await _clientService.GetClientById(new Guid(people));
                var clientChatBox = new ClientChatBox
                {
                    ChatBoxId = new Guid(chatBoxId),
                    ClientId = new Guid(people),
                    IsBlocked = false,
                    IsLeft = false,
                    IsNotificationOn = true,
                    IsModerator = false,
                    NickName = person.Name
                };
                await _clientChatBoxService.AddClientToGroup(clientChatBox);
                var mess = new Message
                {
                    Content = $"{Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_NAME)} added {person.Name} to the group",
                    SentDate = DateTime.Now,
                    SenderId = new Guid(people),
                    MessageType = Domain.Enums.MessageTypes.Notification
                };
                var messToReturn = new MessageDto
                {
                    Content = mess.Content,
                    SenderId = mess.SenderId,
                    SentDate = mess.SentDate,
                    IsYourOwnMess = false,
                    MessageType = mess.MessageType.ToString()
                };
                chatBox.Messages.Add(mess);
                if (_ConnectionPresences.Any(x => x.UserId.Equals(people)))
                {
                    await _chatBoxService.UpdateChatBox(chatBox);
                    var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(new Guid(people));
                    ///
                    var currentUserConnection = _ConnectionPresences.Where(x => x.UserId.Equals(people)).Select(x => x.ConnectionId).ToList();
                    foreach (var connection in currentUserConnection)
                    {
                        await Clients.Client(connection).SendAsync("InitializeChat", await GetClientChatBox(people, clientChatBoxes));
                    }
                    ///
                }
                var currentUserConnectionOfCurrentUser = _ConnectionRooms
                    .Where(x => x.UserId.Equals(Context.GetHttpContext()?
                    .Session.GetString(SessionConstants.USER_ID)) && x.ChatBoxId.Equals(chatBoxId))
                    .ToList();
                foreach (var connection in currentUserConnectionOfCurrentUser)
                {
                    await Clients.Client(connection.ConnectionId).SendAsync("ReceiveMessage",
                        Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_NAME), messToReturn);
                }
            }
        }

        public async Task DeleteGroupChat(string chatBoxId)
        {
            var clientInChat = await _clientChatBoxService.GetClientOfChatBoxes(new Guid(chatBoxId));

            await _chatBoxService.DeleteChatBox(new Guid(chatBoxId));

            foreach (var client in clientInChat)
            {

                if (_ConnectionPresences.Any(x => x.UserId.Equals(client.ClientId.ToString())))
                {

                    var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(client.ClientId);
                    ///
                    var currentUserConnection = _ConnectionPresences.Where(x => x.UserId.Equals(client.ClientId.ToString())).Select(x => x.ConnectionId).ToList();
                    foreach (var connection in currentUserConnection)
                    {
                        await Clients.Client(connection).SendAsync("InitializeChat", await GetClientChatBox(client.ClientId.ToString(), clientChatBoxes));
                    }
                    ///
                }
            }

        }


        private async Task<IList<ClientChatBoxDto>> GetClientChatBox(string userId, IList<ClientChatBox> clientChatBoxes)
        {

            IList<ClientChatBoxDto> returnList = new List<ClientChatBoxDto>();
            foreach (var x in clientChatBoxes)
            {
                //if chatboxname in chatbox table is null or empty, chatbox name = all client in chat box (chatboxclient)
                string chatBoxName;
                Guid clientIdToSend = new Guid(userId);
                if (x.ChatBox.Type == Domain.Enums.ChatBoxType.TwoPerson)
                {
                    chatBoxName = await _clientChatBoxService
                        .GetChatBoxNameOfTwoPersonType(x.ChatBoxId, new Guid(userId));
                    var clientList = await _clientChatBoxService.GetClientOfChatBoxes(x.ChatBoxId);
                    var otherClient = clientList.Select(c => c.Client).Where(c => c.Id.ToString() != userId.ToString()).FirstOrDefault();
                    if (otherClient != null)
                    {
                        clientIdToSend = otherClient.Id;
                        x.ChatBox.ChatBoxAvatar = otherClient.ProfilePicture;
                    }
                }
                else if (!string.IsNullOrEmpty(x.ChatBox.ChatBoxName))
                {
                    chatBoxName = x.ChatBox.ChatBoxName;
                }
                else
                {
                    var clientListInChatBox = await _clientChatBoxService.GetClientOfChatBoxes(x.ChatBoxId);

                    chatBoxName = string.Join(",", clientListInChatBox.Select(c => c.Client.Name).ToList());
                }
                var chatBox = new ClientChatBoxDto
                {
                    ChatBoxId = x.ChatBoxId,
                    ChatBoxAvatar = x.ChatBox.ChatBoxAvatar,
                    ChatBoxName = chatBoxName,
                    ChatBoxType = x.ChatBox.Type.ToString(),
                    ClientId = clientIdToSend,
                    IsLeft = x.IsLeft,
                    IsModerator = x.IsModerator
                };
                returnList.Add(chatBox);
            }
            return returnList;
        }

        public async Task GetClientsOfChatBox(Guid chatBoxId)
        {
            var clientChatBoxes = await _clientChatBoxService.GetClientOfChatBoxes(chatBoxId);
            var clients = clientChatBoxes.Select(c => c.Client);
            var userId = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_ID);
            var clientChatBox = await _clientChatBoxService.GetClientOfChatBoxes(chatBoxId, new Guid(userId));
            await Clients.Caller.SendAsync("ShowClientsOfChatBox", clients, chatBoxId, clientChatBox.FirstOrDefault()?.IsModerator);
        }

        public async Task GetFriendsInChat(Guid chatBoxId)
        {
            var userId = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_ID);
            var clientChatBoxes = await _clientChatBoxService.GetClientOfChatBoxes(chatBoxId);
            var clients = clientChatBoxes.Select(c => c.Client).Where(x => !x.Id.Equals(new Guid(userId)));

            await Clients.Caller.SendAsync("showFriendsListModalLeader", clients, chatBoxId);
        }
        public async Task TranferLeaderPosition(string clientLeaderId, Guid chatBoxId)
        {
            var userId = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_ID);
            var userName = Context.GetHttpContext()?.Session.GetString(SessionConstants.USER_NAME);
            var clientChatBoxes = _clientChatBoxService.GetClientOfChatBoxes(chatBoxId, new Guid(userId)).Result.FirstOrDefault();
            clientChatBoxes.IsModerator = false;
            await _clientChatBoxService.Update(clientChatBoxes);
            var newLeaderClientChat = _clientChatBoxService.GetClientOfChatBoxes(chatBoxId, new Guid(clientLeaderId)).Result.FirstOrDefault();
            newLeaderClientChat.IsModerator = true;
            await _clientChatBoxService.Update(newLeaderClientChat);
            var chatBox = await _chatBoxService.GetChatBoxAsync(chatBoxId);
            var newLeader = await _clientService.GetClientById(new Guid(clientLeaderId));
            var notiMess = new Message
            {
                Content = $"{userName} " +
                $"tranfered leader group position for {newLeader.Name}",
                SentDate = DateTime.Now,
                SenderId = new Guid(userId),
                MessageType = Domain.Enums.MessageTypes.Notification
            };
            chatBox.Messages.Add(notiMess);
            var messToReturn = new MessageDto
            {
                Content = notiMess.Content,
                SenderId = notiMess.SenderId,
                SentDate = notiMess.SentDate,
                IsYourOwnMess = true,
                MessageType = notiMess.MessageType.ToString()
            };
            await _chatBoxService.UpdateChatBox(chatBox);
            await Clients.Group(chatBoxId.ToString()).SendAsync("ReceiveMessage", userName
                , messToReturn);
            var currentUserConnectionOfCurrentUser = _ConnectionRooms
                   .Where(x => x.UserId.Equals(userId) && x.ChatBoxId.Equals(chatBoxId.ToString()))
                   .ToList();
            var chatBoxUpdate = await GetClientChatBox(userId, await _clientChatBoxService.GetClientOfChatBoxes(chatBoxId, new Guid(userId)));
            foreach (var connection in currentUserConnectionOfCurrentUser)
            {
                await Clients.Client(connection.ConnectionId).SendAsync("UpdateMessagesForChatBox",
                    chatBoxUpdate.FirstOrDefault());
            }


        }
    }

}
