using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TalkBuddy.Presentation.Pages.Moderator;

public class Index : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("Reports/Index");
    }
}