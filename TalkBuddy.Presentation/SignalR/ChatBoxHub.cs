using Microsoft.AspNetCore.SignalR;
using TalkBuddy.DAL.Interfaces;

namespace TalkBuddy.Presentation.SignalR
{
    public class ChatBoxHub : Hub
    {
        private readonly IUnitOfWork _uow;
        public ChatBoxHub(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var chatBoxId = httpContext.Request.Query["chatBox"];
           // await Groups.AddToGroupAsync()
        }
    }
}
