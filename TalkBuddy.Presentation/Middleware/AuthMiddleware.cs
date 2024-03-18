using Microsoft.AspNetCore.Authentication;
using TalkBuddy.Common.Constants;
using TalkBuddy.Common.Enums;
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
					context.Response.Redirect($"/{nameof(ChatPage)}");
					return Task.CompletedTask;
				}

				return _next(context);
			}

			if (isLoggedIn == SessionConstants.LOGGED_IN)
			{
				if (context.Request.Path.HasValue
				    && context.Request.Path.Value.StartsWith("/Moderator")
				    && context.Session.GetString(SessionConstants.USER_ROLE) != UserRole.MODERATOR.ToString())
				{
					context.Response.Redirect("/");
					return Task.CompletedTask;
				}
				
				return _next(context);
			}

			context.Response.Redirect($"/{nameof(Login)}");
			return Task.CompletedTask;
		}

		private bool IsAuthenticationPath(string path)
		{
			return path.StartsWith("/Login", StringComparison.OrdinalIgnoreCase)
			       || path.StartsWith("/OAuth", StringComparison.OrdinalIgnoreCase)
			       || path.StartsWith("/Register", StringComparison.OrdinalIgnoreCase)
			       || path.StartsWith("/ConfirmOtp", StringComparison.OrdinalIgnoreCase);
		}
	}
}
