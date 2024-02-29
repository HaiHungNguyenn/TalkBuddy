using Microsoft.AspNetCore.SignalR;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;
using TalkBuddy.Domain.Dtos;

namespace TalkBuddy.Presentation.SignalR
{
    public class ChatHub : Hub
    {
        public readonly static List<UserConnection> _ConnectionRooms = new List<UserConnection>();
        private readonly static List<string> _ConnectionPresences = new List<string>();

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
               
                await Clients.Caller.SendAsync("InitializeChat", await GetClientChatBox(userId));
                if (!_ConnectionPresences.Contains(userId))
                {
                    _ConnectionPresences.Add(userId);
                }
            }
          
            base.OnConnectedAsync();
        }



        public override Task OnDisconnectedAsync(Exception ex)
        {
            try
            {
                var httpContext = Context.GetHttpContext();

                var userId = httpContext.Session.GetString(SessionConstants.USER_ID);
                var user = _ConnectionPresences.Where(u => u.Equals(userId)).First();
                _ConnectionPresences.Remove(user);
                foreach (var room in _ConnectionRooms.Where(x => x.UserId.Equals(userId) && x.ConnectionId.Equals(httpContext.Connection.Id)))
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
            if (_ConnectionPresences.Contains(userId))
            {
                List<UserConnection> userInChatList = _ConnectionRooms.Where(x => x.UserId.Equals(userId)
                                                && !x.ChatBoxId.Equals(chatBoxId)
                                                && x.ConnectionId.Equals(Context.ConnectionId)).ToList();
                if (userInChatList!=null && userInChatList.Any())
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
                //}
                // Retrieve messages from a data source (e.g., database)
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

                    };
                    messageReturns.Add(mess);
                }
                //return messageReturns;
            }
            return messageReturns;
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
            await _messageService.AddMessage(messageObject);
            //if (!IsChatBoxContainMessages)
            //{
            //    await Clients.Caller.SendAsync("InitializeChat", await GetClientChatBox(fromUserId));
            //}

            await Clients.Group(chatBoxId).SendAsync("ReceiveMessage", sender.Name, message);

        }

        private async Task<IList<ClientChatBoxDto>> GetClientChatBox(string userId)
        {
            var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(new Guid(userId));
            IList<ClientChatBoxDto> returnList = new List<ClientChatBoxDto>();
            foreach (var x in clientChatBoxes)
            {
                //if chatboxname in chatbox table is null or empty, chatbox name = all client in chat box (chatboxclient)
                string chatBoxName;
                if (!string.IsNullOrEmpty(x.ChatBox.ChatBoxName))
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
                    ChatBoxName = chatBoxName
                };
                returnList.Add(chatBox);
            }
            return returnList;
        }
    }
        
}
