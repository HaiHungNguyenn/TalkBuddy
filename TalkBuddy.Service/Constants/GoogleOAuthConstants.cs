using TalkBuddy.Service.Settings;

namespace TalkBuddy.Service.Constants
{
	public static class GoogleOAuthConstants
	{
		public const string CODE = "code";
		public const string CLIENT_ID = "client_id";
		public const string CLIENT_SECRET = "client_secret";
		public const string REDIRECT_URI = "redirect_uri";
		public const string GRANT_TYPE = "grant_type";
		public const string AUTHORIZATION_CODE = "authorization_code";
		public const string STATE = "state";
		public const string SCOPE = "scope";
		public const string AUTH_USER= "authuser";
		public const string HD = "hd";
		public const string PROMPT = "prompt";
		public const string GOOGLE_TOKEN_URL = "https://oauth2.googleapis.com/token";
		
		public static string BuildGoogleOauthUrl(GoogleSettings googleSettings)
		{
			return $"https://accounts.google.com/o/oauth2/v2/auth?client_id={googleSettings.ClientId}&scope=email%20profile%20openid&redirect_uri={googleSettings.RedirectUri}&response_type=code";
		}
	}
}
