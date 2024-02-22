using Microsoft.AspNetCore.Authentication;
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
			if (context.Request.Path.Value != null && IsAuthenticationPath(context.Request.Path.Value))
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

			context.Response.Redirect("/Login");
			return Task.CompletedTask;
		}

		private bool IsAuthenticationPath(string path)
		{ 
			return path.StartsWith("/Login", StringComparison.OrdinalIgnoreCase) || path.StartsWith("/OAuth", StringComparison.OrdinalIgnoreCase);
		}
	}
}
