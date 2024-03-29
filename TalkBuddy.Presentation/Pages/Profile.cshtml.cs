using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages;

public class Profile : PageModel
{
    private readonly IGenericRepository<Client> _clientRepo;
    private readonly IUnitOfWork _unitOfWork;

    public Profile(IClientService clientService, IGenericRepository<Client> clientRepo, IUnitOfWork unit)
    {
        _clientRepo = clientRepo;
        _unitOfWork = unit;
    }
    public async Task<IActionResult> OnGet(string id)
    {
        //check is user logged in
        var userId = HttpContext.Session.GetString(SessionConstants.USER_ID);
        if(userId == null) return RedirectToPage("/Login");
        //check user in db
        var user = await _clientRepo.GetAsync(u => u.Id.ToString().Equals(id));
        if(user == null) return RedirectToPage("/Login");
        CurrentUser = user;
        if(CurrentUser.ProfilePicture != null) ProfilePicture = CurrentUser.ProfilePicture;
        return Page();
    }
    [BindProperty]
    public Client CurrentUser { get; set; }
    public string ProfilePicture {get;set;} = "/default-avatar.png";
}
