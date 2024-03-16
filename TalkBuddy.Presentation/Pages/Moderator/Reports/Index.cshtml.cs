using Microsoft.AspNetCore.Mvc.RazorPages;
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
        TempData["reportsList"] = await _reportService.GetAllReports();
    }
}