using TalkBuddy.Common.Constants;
using TalkBuddy.Presentation.Pages;

namespace TalkBuddy.Presentation.Middleware
{
    public class AuthMiddleware
    {
		private readonly RequestDelegate _next;

		public AuthMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public Task Invoke(HttpContext context)
		{
			var isLoggedIn = context.Session.GetString(SessionConstants.IS_LOGGED_IN) ?? "false";
			if (context.Request.Path.Value!.Contains(nameof(Login)))
			{
				if (isLoggedIn == SessionConstants.LOGGED_IN)
				{
					context.Response.Redirect("/");
					return Task.CompletedTask;
				}

				return _next(context);
			}

			if (isLoggedIn == SessionConstants.LOGGED_IN)
			{
				return _next(context);
			}

			context.Response.Redirect(nameof(Login));
			return Task.CompletedTask;
		}
	}
}
