using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages
{
    public class ChatPage : PageModel
    {
        private readonly IClientChatBoxService _clientChatBoxService;
        [BindProperty]
        public IList<ClientChatBox> ClientChatBoxes { get; set; } = new List<ClientChatBox>();
        public ChatPage(IClientChatBoxService clientChatBoxService)
        {
            _clientChatBoxService = clientChatBoxService;
        }
        public async void OnGet()
        {
            var userId = HttpContext.Session.GetString(SessionConstants.USER_ID);
            ClientChatBoxes = await _clientChatBoxService.GetClientChatBoxes(new Guid(userId));
        }
    }
}
