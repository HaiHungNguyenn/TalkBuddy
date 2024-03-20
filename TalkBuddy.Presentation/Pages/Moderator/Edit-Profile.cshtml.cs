using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Constants;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages.Moderator;

public class EditProfile : PageModel
{
    private readonly IGenericRepository<Client> _clientRepo;
    private readonly IUnitOfWork _unitOfWork;

    public EditProfile(IClientService clientService, IGenericRepository<Client> clientRepo, IUnitOfWork unit)
    {
        _clientRepo = clientRepo;
        _unitOfWork = unit;
    }
    public async Task<IActionResult> OnGet()
    {
        var userId = HttpContext.Session.GetString(SessionConstants.USER_ID);
        if (userId == null) return RedirectToPage("/Login");
        var user = await _clientRepo.GetAsync(u => u.Id.ToString().Equals(userId));
        if (user == null) return RedirectToPage("/Login");
        CurrentUser = user;
        if (CurrentUser.ProfilePicture != null) ProfilePicture = CurrentUser.ProfilePicture;
        return Page();
    }
    [BindProperty]
    public Client CurrentUser { get; set; }
    public string ProfilePicture { get; set; } = "/default-avatar.png";

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Console.WriteLine(CurrentUser.Id);
        CurrentUser.IsVerified = true;
        CurrentUser.Role = Common.Enums.UserRole.MODERATOR;
        await _clientRepo.UpdateAsync(CurrentUser);
        await _unitOfWork.CommitAsync();
        return RedirectToPage("./Edit-Profile");
    }
}
