using Microsoft.AspNetCore.SignalR;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.SignalR
{
    public class ChatHub : Hub
    {
        //public readonly static List<UserConnection> _Connections = new List<UserConnection>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

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
            var user = httpContext.Session.GetString(SessionConstants.USER_ID);
            var chatBoxId = httpContext.Request.Query["chatBoxId"];
            if(!string.IsNullOrEmpty(chatBoxId) )
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId);
            }

            var clientChatBoxes = _clientChatBoxService.GetClientChatBoxes(new Guid(user));
            //load chatbox
            await Clients.Caller.SendAsync("InitializeChat", clientChatBoxes);

            // if (!_Connections.Any(u => u.ChatBoxId == chatBoxId))
            //{
            // _Connections.Add(new UserConnection { ChatBoxId = chatBoxId, UserName = fromUser});
            // _ConnectionsMap.Add(chatBoxId, Context.ConnectionId);
            // }

            
             base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                var chatBoxId = httpContext.Request.Query["chatBoxId"];
                var fromUser = httpContext.Session.GetString(SessionConstants.USER_ID);
               // var user = _Connections.Where(u => u.UserName == fromUser).First();
              //  _Connections.Remove(user);

                // Remove mapping
                //_ConnectionsMap.Remove(user.UserName);
            }
            catch (Exception exep)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + exep.Message);
            }

            return base.OnDisconnectedAsync(ex);
        }

        public async Task<IList<Message>> GetMessages(Guid chatBoxId)
        {
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

            };
            var chatBox = await _chatBoxService.GetChatBoxAsync(new Guid(chatBoxId));
            //if chua co tin nhan load lai chatbox
            //two user have not text each other before, need to load chatbox again
            if (chatBox == null) 
            { 

            }
            await _messageService.AddMessage(messageObject);
         
            await Clients.Group(chatBoxId).SendAsync("NewMessage", message);
           
        }       

           
    }
}
