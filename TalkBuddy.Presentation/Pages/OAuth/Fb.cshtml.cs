using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Constants;
using TalkBuddy.Service.Interfaces;
using TalkBuddy.Service.Models.Requests;

namespace TalkBuddy.Presentation.Pages.OAuth
{
    public class FbModel : PageModel
    {
        private readonly IFacebookOAuthService _facebookOAuthService;

        public FbModel(IFacebookOAuthService facebookOAuthService)
        {
            _facebookOAuthService = facebookOAuthService;
        }

        public async Task<IActionResult> OnGet()
        {
            Request.Query.TryGetValue(FacebookOAuthConstants.ACCESS_TOKEN, out var accessToken);
            Request.Query.TryGetValue(FacebookOAuthConstants.DATA_ACCESS_EXPIRATION_TIME, out var dataAccessExpirationTimeAsString);
            Request.Query.TryGetValue(FacebookOAuthConstants.EXPIRES_IN, out var expiresInAsString);

            var request = new FacebookOAuthRequest
            {
                AccessToken = accessToken.ToString(),
                DataAccessExpirationTime = int.TryParse(dataAccessExpirationTimeAsString, out var dataAccessExpirationTime) ? dataAccessExpirationTime : 0,
                ExpiresIn = int.TryParse(expiresInAsString, out var expiresIn) ? expiresIn : 0,
            };

            try
            {
                var user = await _facebookOAuthService.ContinueWithFacebookAsync(request) ?? throw new Exception("An error occurred");

                HttpContext.Session.SetString(SessionConstants.USER_NAME, user.Name);
                HttpContext.Session.SetString(SessionConstants.USER_ID, user.Id.ToString());
                HttpContext.Session.SetString(SessionConstants.IS_LOGGED_IN, SessionConstants.LOGGED_IN);

                return RedirectToPage($"/{nameof(Index)}");
            }
            catch (Exception ex)
            {
                return RedirectToPage($"/{nameof(Login)}", nameof(Login.OnGet), new { error = ex.Message });
            }
        }
    }
}
