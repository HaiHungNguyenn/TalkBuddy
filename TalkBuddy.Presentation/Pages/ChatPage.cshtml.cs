using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages
{
    public class ChatPage : PageModel
    {
        private readonly IClientChatBoxService _clientChatBoxService;
        public ChatPage(IClientChatBoxService clientChatBoxService)
        {

            _clientChatBoxService = clientChatBoxService;

        }
        public IActionResult OnGetLogOut()
        {
            HttpContext.Session.Remove(SessionConstants.IS_LOGGED_IN);
            HttpContext.Session.Remove(SessionConstants.USER_ID);
            HttpContext.Session.Remove(SessionConstants.USER_NAME);

            return RedirectToRoute("");
        }
        
        public async Task<IActionResult> OnGetGetFriendsListNotInChat(string chatboxId, string userId)
        {
            if (string.IsNullOrEmpty(chatboxId) || string.IsNullOrEmpty(userId))
            {
                return BadRequest("ChatboxId and UserId must be provided.");
            }

            try
            {
                // Assuming _clientChatBoxService.GetFriendsNotInChatBoxes returns a list of friends
                var friends = await _clientChatBoxService.GetFriendsNotInChatBoxes(new Guid(chatboxId), new Guid(userId));

                return new JsonResult(friends);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
