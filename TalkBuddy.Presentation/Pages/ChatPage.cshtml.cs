using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using TalkBuddy.Common.Constants;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages
{
    public class ChatPage : PageModel
    {
        public IActionResult OnGetLogOut()
        {
            HttpContext.Session.Remove(SessionConstants.IS_LOGGED_IN);
            HttpContext.Session.Remove(SessionConstants.USER_ID);
            HttpContext.Session.Remove(SessionConstants.USER_NAME);

            return RedirectToRoute("");
        }
    }
}
