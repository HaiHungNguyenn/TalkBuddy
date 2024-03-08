using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TalkBuddy.Common.Constants;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Presentation.SignalR;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages;

public class AddFriendRequest : PageModel
{
    private readonly IFriendShipService _friendShipService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationService _notificationService;
    private readonly IClientService _clientService;


    public AddFriendRequest(IFriendShipService friendShipService, IHubContext<NotificationHub> hubContext,INotificationService notificationService,IClientService clientService)
    {
        _friendShipService = friendShipService;
        _hubContext = hubContext;
        _notificationService = notificationService;
        _clientService = clientService;
    }


    [BindProperty]
    public Guid FriendShipId { get; set; }
    [BindProperty]

    public Guid SenderId { get; set; }
    [BindProperty]
    public Guid ReceiverId { get; set; }


    public async Task OnGet()
    {
        var clientId = HttpContext.Session.GetString(SessionConstants.USER_ID)!;
        var invitationList = (await _friendShipService.GetFriendInvitation(new Guid(clientId))).Include(c => c.Sender);
        TempData["invitationList"] = invitationList.AsEnumerable();
    }
}
