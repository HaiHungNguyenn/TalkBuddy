using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages
{
    public class FriendsListModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IFriendShipService _friendShipService;

        public string Message { get; set; } = string.Empty;

        public FriendsListModel(IClientService clientService, IFriendShipService friendShipService)
        {
            _clientService = clientService;
            _friendShipService = friendShipService;
        }

        public async Task OnGet()
        {
            await LoadFriends();
        }

		public IActionResult OnPostOpenChat(Guid friendId)
		{
			return RedirectToPage($"/{nameof(ChatPage)}");
		}

        public async Task<IActionResult> OnPostRemoveFriend(Guid friendId)
        {
            try
            {
                await _friendShipService.DeleteFriendShip(friendId, Guid.Parse(HttpContext.Session.GetString(SessionConstants.USER_ID) ?? string.Empty));
                Message = "Friend deleted successfully";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            await LoadFriends();
            return Page();
        }

        private async Task LoadFriends()
        {
            var client = await _clientService.GetClientById(Guid.Parse(HttpContext.Session.GetString(SessionConstants.USER_ID) ?? string.Empty));
            TempData["friendsList"] = await _friendShipService.GetClientFriends(client!.Id);
        }
    }
}
