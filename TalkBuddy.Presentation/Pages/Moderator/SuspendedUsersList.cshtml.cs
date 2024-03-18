using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages.Moderator;

public class SuspendedUsersList : PageModel
{
    private readonly IReportService _reportService;

    public SuspendedUsersList(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IActionResult> OnGet()
    {
        TempData["suspendedUsers"] = await (await _reportService.GetSuspendedClient()).ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostUnbanUser(Guid userId)
    {
        await _reportService.UnbanUser(userId);

        return Page();
    }
}