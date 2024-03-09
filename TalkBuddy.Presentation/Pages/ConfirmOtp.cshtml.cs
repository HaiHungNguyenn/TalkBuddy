using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages;

public class ConfirmOtp : PageModel
{
    private readonly IClientService _clientService;

	public ConfirmOtp(IClientService clientService)
	{
		_clientService = clientService;
	}

	public string ErrorMessage { get; set; } = string.Empty;
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostConfirm(string code)
    {
        try
        {
            var clientReturned = await _clientService.ConfirmCodeAsync(code);

			HttpContext.Session.SetString(SessionConstants.USER_NAME, clientReturned.Name);
			HttpContext.Session.SetString(SessionConstants.USER_ID, clientReturned.Id.ToString());
			HttpContext.Session.SetString(SessionConstants.IS_LOGGED_IN, SessionConstants.LOGGED_IN);

			return RedirectToPage(nameof(ChatPage));

		}
		catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }

    } 
}