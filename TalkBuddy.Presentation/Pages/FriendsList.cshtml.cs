using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages
{
    public class FriendsListModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IFriendShipService _friendShipService;

        public FriendsListModel(IClientService clientService, IFriendShipService friendShipService)
        {
            _clientService = clientService;
            _friendShipService = friendShipService;
        }

        public async Task OnGet()
        {
            var client = await _clientService.GetClientById(Guid.Parse(HttpContext.Session.GetString(SessionConstants.USER_ID) ?? string.Empty));
            TempData["friendsList"] = await _friendShipService.GetClientFriends(client!.Id);
        }
    }
}
