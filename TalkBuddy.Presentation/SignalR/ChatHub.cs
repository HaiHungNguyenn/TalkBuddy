using Microsoft.AspNetCore.SignalR;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.SignalR
{
    public class ChatHub : Hub
    {
        public readonly static List<UserConnection> _Connections = new List<UserConnection>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

        private readonly IChatBoxService _chatBoxService;
        public ChatHub(IChatBoxService chatBoxService)
        {
            _chatBoxService = chatBoxService;
        }
     
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var fromUser = httpContext.Session.GetString(SessionConstants.USER_ID);
            var chatBoxId = httpContext.Request.Query["chatBoxId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId);

            if (!_Connections.Any(u => u.ChatBoxId == chatBoxId))
            {
                _Connections.Add(new UserConnection { ChatBoxId = chatBoxId, UserName = fromUser});
                _ConnectionsMap.Add(chatBoxId, Context.ConnectionId);
            }
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                var chatBoxId = httpContext.Request.Query["chatBoxId"];
                var fromUser = httpContext.Session.GetString(SessionConstants.USER_ID);
                var user = _Connections.Where(u => u.UserName == fromUser).First();
                _Connections.Remove(user);

                // Tell other users to remove you from their list
                Clients.OthersInGroup(user.ChatBoxId).SendAsync("removeUser", user);

                // Remove mapping
                _ConnectionsMap.Remove(user.UserName);
            }
            catch (Exception exep)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + exep.Message);
            }

            return base.OnDisconnectedAsync(ex);
        }

        public async Task SendMessage(Message message, string chatBoxId)
        {
            var httpContext = Context.GetHttpContext();
            var fromUser = httpContext.Session.GetString(SessionConstants.USER_ID);

            var chatBox = await _chatBoxService.GetChatBoxAsync(new Guid(chatBoxId));
            if (chatBox == null) throw new HubException("Not found conversation");

            await Groups.AddToGroupAsync(Context.ConnectionId, fromUser);
           
           await Clients.Group(chatBoxId).SendAsync("NewMessage", message);
           
        }       

           
    }
}
