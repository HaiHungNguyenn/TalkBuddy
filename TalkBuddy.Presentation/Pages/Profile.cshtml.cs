using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages;

public class Profile : PageModel
{
    private readonly IClientService _clientService;

    public Profile(IClientService clientService)
    {
        _clientService = clientService;
    }
    public void OnGet()
    {

    }
    
    [BindProperty]
    public string UserName { get; set; }

    public async Task OnPost()
    {
        RedirectToPage(Page());
    }
}
