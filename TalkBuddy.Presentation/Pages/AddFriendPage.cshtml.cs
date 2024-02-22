using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Domain.Dtos;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages;

public class AddFriendPage : PageModel
{
    private readonly IClientService _clientService;

    private List<Client> clientList;


    public AddFriendPage( IClientService clientService)
    {
        _clientService = clientService;
        clientList = new List<Client>
        {
            new Client
            {
                Name = "ABC",
                Email = "XYZ",
                Password = "password" 
            },
            new Client
            {
                Name = "ABC1",
                Email = "XYZ",
                Password = "password" 
            },
            new Client
            {
                Name = "ABC2",
                Email = "XYZ",
                Password = "password" 
            },
        };
    }
    public void OnGet()
    {

    }
    
    [BindProperty]
    public string UserName { get; set; }
    
    [BindProperty]
    public Guid ClientID { get; set; }

    public async Task OnPost()
    {
        var list = (await _clientService.FindClient(UserName)).ToList();

        var dtoList = new List<DtoClientForFriend>();
        foreach (var client in list)
        {
            var x = new DtoClientForFriend()
            {
                id = client.Id,
                Email = client.Email,
                Name = client.Name,
                // isFriend = operator.Friends.Any(clientFriend => clientFriend.Client2Id == ClientID)
            };
            dtoList.Add(x);
        }
        
        TempData["friendList"] = dtoList;
        // TempData["friendList"] = clientList;
        RedirectToPage(Page());
    }

    public async Task OnPostHandleAddFriend()
    {
        Console.WriteLine(ClientID);
    }
}