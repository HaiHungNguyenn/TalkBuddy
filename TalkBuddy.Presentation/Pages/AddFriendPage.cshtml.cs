using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.Common.Enums;
using TalkBuddy.Domain.Dtos;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages;

public class AddFriendPage : PageModel
{
    private readonly IClientService _clientService;
    private readonly IFriendShipService _friendShipService;

    private List<Client> clientList;

    public AddFriendPage( IClientService clientService, IFriendShipService friendShipService)
    {
        _clientService = clientService;
        _friendShipService = friendShipService;
    }
    public void OnGet()
    {

    }
    
    [BindProperty]
    public string UserName { get; set; }
    
    [BindProperty]

    public Guid FriendId { get; set; }

    public Guid ClientID { get; set; }


    public async Task<PageResult> OnPost()
    {
        var list = (await _clientService.FindClient(UserName)).ToList();

        var clientId = new Guid(HttpContext.Session.GetString(SessionConstants.USER_ID!)!) ;
        var dtoList =  list.Select( x => new DtoClientForFriend()
        {
            id = x.Id,
            Email = x.Email,
            Name = x.Name,
            RelationStatus = (clientId.Equals(x.Id)) ? FriendShipRequestStatus.YOURSELF : _friendShipService.GetFriendShipStatus(clientId,x.Id) 
        });
        TempData["friendList"] = dtoList;
        return Page();
    }

    public async Task<IActionResult> OnPostHandleAddFriend()
    {
        var clientId = new Guid(HttpContext.Session.GetString(SessionConstants.USER_ID!)!) ;
        var x = FriendId;
        await _friendShipService.CreateFriendShip(new Friendship() {SenderID = clientId,ReceiverId = FriendId, RequestDate = DateTime.Today});
		return Page();
    }

    public async Task<RedirectToPageResult> OnPostHandleCancelInvitation()
    {
        var clientId = new Guid(HttpContext.Session.GetString(SessionConstants.USER_ID!)!) ;
        var x = FriendId;
        await _friendShipService.CancelInvitation(clientId, x);
        return RedirectToPage("/AddFriendPage");
    }

}
