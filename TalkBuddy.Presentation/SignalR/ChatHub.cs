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
         
        private readonly IChatBoxService _chatBoxService;
        private readonly IMessageService _messageService;
        private readonly IClientChatBoxService _clientChatBoxService;

        public ChatHub(IChatBoxService chatBoxService,
            IMessageService messageService,
            IClientChatBoxService clientChatBoxService)
        {
            _chatBoxService = chatBoxService;
            _messageService = messageService;
            _clientChatBoxService = clientChatBoxService;
        }

        public override async Task OnConnectedAsync()
        {
          
                var httpContext = Context.GetHttpContext();
                var userId = httpContext.Session.GetString(SessionConstants.USER_ID);
                List<ClientChatBoxDto> returnList = new List<ClientChatBoxDto>();
                if (!string.IsNullOrEmpty(userId))
                {
                    var clientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(new Guid(userId));
                    foreach(var x in clientChatBoxes)
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
                    await Clients.Caller.SendAsync("InitializeChat", returnList);
                if (!_ConnectionPresences.Contains(userId))
                {
                    _ConnectionPresences.Add(userId);
                }
            }
                else
                {
                    // Handle the case where the user ID is null or empty
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
                foreach (var room in _ConnectionRooms.Where(x => x.UserId.Equals(userId)&&x.ConnectionId.Equals(httpContext.Connection.Id)))
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

        public async Task<IList<Message>> GetMessages(Guid chatBoxId)
        {
            var httpContext = Context.GetHttpContext();         
            var userId = httpContext.Session.GetString(SessionConstants.USER_ID);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId.ToString());
            if (_ConnectionPresences.Contains(userId))
            {
                if (_ConnectionRooms.Where(x => x.UserId.Equals(userId)
                                                &&!x.ChatBoxId.Equals(chatBoxId)
                                                &&x.ConnectionId.Equals(httpContext.Connection.Id))
                                                .Any())
                {                    foreach(var room in _ConnectionRooms.Where(x => x.UserId.Equals(userId) && !x.ChatBoxId.Equals(chatBoxId) && x.ConnectionId.Equals(httpContext.Connection.Id)))
                    {
                        _ConnectionRooms.Remove(room);
                    }
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatBoxId.ToString());
                    await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId.ToString());

                }
                
            }
            // Retrieve messages from a data source (e.g., database)
            var messages = await _messageService.GetMessages(chatBoxId);
            return messages;
        }

        public async Task SendMessage(string message, string chatBoxId)
        {
            var httpContext = Context.GetHttpContext();
            var fromUser = httpContext.Session.GetString(SessionConstants.USER_ID);

            Message messageObject = new Message
            {
                Content = message,
                SentDate = DateTime.Now,
                ChatBoxId = new Guid(chatBoxId)
            };
            var chatBox = await _chatBoxService.GetChatBoxAsync(new Guid(chatBoxId));
            //if chua co tin nhan load lai chatbox
            //two user have not text each other before, need to load chatbox again
            //if (chatBox != null) 
            //{
            //    if (!chatBox.Messages.Any())
            //    {
            //        var clientChatBoxes = _clientChatBoxService.GetClientChatBoxes(new Guid(receiverId)).Result;
            //        if(_ConnectionPresences.Contains(receiverId))
            //        {
            //            var receiver=_ConnectionRooms.Where(x => x.ChatBoxId.EndsWith(chatBoxId)&&x.UserId).FirstOrDefault();
            //            await Clients.Clients().SendAsync("InitializeChat", clientChatBoxes);
            //        }
                    
            //    }
            //}
            await _messageService.AddMessage(messageObject);
         
            await Clients.Group(chatBoxId).SendAsync("NewMessage", message);
           
        }       

           
    }
}
