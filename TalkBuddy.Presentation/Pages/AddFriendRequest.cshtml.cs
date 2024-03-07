using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages;

public class AddFriendRequest : PageModel
{
    private readonly IFriendShipService _friendShipService;

    public AddFriendRequest(IFriendShipService friendShipService)
    {
        _friendShipService = friendShipService;
    }
    [BindProperty]
    public Guid FriendShipId { get; set; }

    public async Task OnGet()
    {
        var clientId = HttpContext.Session.GetString(SessionConstants.USER_ID)!;
        var invitationList =  (await _friendShipService.GetFriendInvitation(new Guid(clientId))).Include(c => c.Sender);
        TempData["invitationList"] = invitationList.AsEnumerable();
    }

    public async Task<RedirectToPageResult> OnPostHandleAccept()
    {
        try
        {
            await _friendShipService.AcceptFriendInvitation(FriendShipId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return RedirectToPage("/AddFriendRequest");
    } 
    public async Task<RedirectToPageResult> OnPostHandleReject()
    {
        try
        {
            await _friendShipService.RejectFriendInvitation(FriendShipId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        } 
        
        return RedirectToPage("/AddFriendRequest");
    }
}
