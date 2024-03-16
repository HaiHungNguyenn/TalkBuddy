using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Common.Enums;
using TalkBuddy.Service.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages;

public class Login : PageModel
{
    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    private readonly IClientService _clientService;

    public string GoogleOAuthUrl { get; } = GoogleOAuthConstants.GOOGLE_OAUTH_URL;

    public string FacebookOAuthUrl { get; } = FacebookOAuthConstants.FACEBOOK_OAUTH_URL;

	public Login(IClientService clientService)
	{
		_clientService = clientService;
	}

    public void OnGet()
    {
        ErrorMessage = Request.Query["error"].ToString() ?? string.Empty;
    }

    public IActionResult OnPostClearMessage()
    {
        ErrorMessage = string.Empty;
        return RedirectToPage(nameof(Login));
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            var user = await _clientService.LoginAsync(Username, Password) ?? throw new Exception("Invalid username or password");

            HttpContext.Session.SetString(SessionConstants.USER_NAME, user.Name);
            HttpContext.Session.SetString(SessionConstants.USER_ID, user.Id.ToString());
            HttpContext.Session.SetString(SessionConstants.USER_ROLE, user.Role.ToString());
            HttpContext.Session.SetString(SessionConstants.IS_LOGGED_IN, SessionConstants.LOGGED_IN);
            HttpContext.Response.Cookies.Append("userId", user.Id.ToString());

            return RedirectToPage(user.Role == UserRole.MODERATOR ? "/Moderator/Index" : nameof(ChatPage));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}