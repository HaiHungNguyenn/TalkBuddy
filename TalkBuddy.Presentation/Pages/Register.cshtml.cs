using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages
{
	public class Register : PageModel
    {
		[BindProperty]
		public string Username { get; set; } = string.Empty;

		[BindProperty]
		public string Password { get; set; } = string.Empty;
		
		[BindProperty]
		public string ConfirmPassword { get; set; } = string.Empty;

		public string ErrorMessage { get; set; } = string.Empty;

		public string GoogleOAuthUrl { get; } = GoogleOAuthConstants.GOOGLE_OAUTH_URL;

		private readonly IClientService _clientService;

        public Register(IClientService clientService)
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
			return RedirectToPage(nameof(Register));
		}

		public async Task<IActionResult> OnPost()
		{
			try
			{
				if (!string.Equals(Password, ConfirmPassword))
					throw new Exception("Password does not match");

				await _clientService.RegisterAsync(Username, Password);

				return RedirectToPage(nameof(ConfirmOtp));				
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
				return Page();
			}
		}
    }
}