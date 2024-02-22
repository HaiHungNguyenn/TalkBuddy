using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages;

public class AddFriendPage : PageModel
{
    private readonly IClientService _clientService;

    public AddFriendPage( IClientService clientService)
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
        var list = (await _clientService.FindClient(UserName)).ToList();
        TempData["friendList"] = list;
        RedirectToPage(Page());
    }
}