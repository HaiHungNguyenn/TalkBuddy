using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Constants;
using TalkBuddy.Service.Interfaces;
using TalkBuddy.Service.Models.Requests;

namespace TalkBuddy.Presentation.Pages.OAuth
{
    public class GgModel : PageModel
    {
        private readonly IGoogleOAuthService _googleOAuthService;

        public GgModel(IGoogleOAuthService googleOAuthService)
        {
            _googleOAuthService = googleOAuthService;
        }

        public async Task<IActionResult> OnGet()
        {
            Request.Query.TryGetValue(GoogleOAuthConstants.CODE, out var code);
            Request.Query.TryGetValue(GoogleOAuthConstants.HD, out var hd);
            Request.Query.TryGetValue(GoogleOAuthConstants.SCOPE, out var scope);
            Request.Query.TryGetValue(GoogleOAuthConstants.AUTH_USER, out var authUser);
            Request.Query.TryGetValue(GoogleOAuthConstants.PROMPT, out var prompt);
            Request.Query.TryGetValue(GoogleOAuthConstants.STATE, out var state);

            var request = new GoogleOAuthRequest
            {
                Code = code.ToString(),
                Hd = hd.ToString(),
                Scope = scope.ToString(),
                Authuser = authUser.ToString(),
                Prompt = prompt.ToString(),
                State = state.ToString()
            };

            try
            {
                var user = await _googleOAuthService.ContinueWithGoogleAsync(request) ?? throw new Exception("An error occurred");

                HttpContext.Session.SetString(SessionConstants.USER_NAME, user.Name);
                HttpContext.Session.SetString(SessionConstants.USER_ID, user.Id.ToString());
                HttpContext.Session.SetString(SessionConstants.IS_LOGGED_IN, SessionConstants.LOGGED_IN);
                HttpContext.Session.SetString(SessionConstants.USER_PROFILE_IMAGE, user.ProfilePicture ?? "/default-avatar.png");

                return RedirectToPage($"/{nameof(ChatPage)}");
            }
            catch (Exception ex)
            {
                return RedirectToPage($"/{nameof(Login)}", nameof(Login.OnGet), new { error = ex.Message });
            }
        }
    }
}
