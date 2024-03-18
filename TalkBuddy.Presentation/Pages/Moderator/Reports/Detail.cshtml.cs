using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages.Moderator.Reports;

public class Detail : PageModel
{
    private readonly IReportService _reportService;

    public Detail(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task OnGet(Guid reportId)
    {
        TempData["reportDetail"] = await _reportService.GetReportById(reportId);
    }

    public async Task<IActionResult> OnPostDismissReport(Guid reportId)
    {
        try
        {
            await _reportService.DismissReport(reportId);
            return Redirect("/Moderator/Reports");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Page();
        }
    }
}