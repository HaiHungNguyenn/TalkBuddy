using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Common.Enums;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages.Moderator.Reports;

public class Index : PageModel
{
    private readonly IReportService _reportService;

    public Index(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task OnGet()
    {
        TempData["reportsList"] = await _reportService.GetWaitingReports();
    }

    public async Task<IActionResult> OnPostDismissReport(Guid reportId)
    {
        try
        {
            await _reportService.DismissReport(reportId);
            return RedirectToAction(nameof(OnGet));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}