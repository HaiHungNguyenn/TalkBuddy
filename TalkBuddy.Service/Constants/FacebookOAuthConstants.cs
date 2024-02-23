namespace TalkBuddy.Service.Constants
{
	public static class FacebookOAuthConstants
	{
		public const string FACEBOOK_GRAPH_API_URL = "https://graph.facebook.com/v19.0/me";
		public const string ACCESS_TOKEN = "access_token";
		public const string FIELDS = "fields";
		public const string ID = "id";
		public const string NAME = "name";
		public const string EMAIL = "email";
		public const string DATA_ACCESS_EXPIRATION_TIME = "data_access_expiration_time";
		public const string EXPIRES_IN = "expires_in";
		public const string FACEBOOK_OAUTH_URL = "https://www.facebook.com/v19.0/dialog/oauth?client_id=937598707932566&scope=email%2Cpublic_profile&response_type=token&redirect_uri=http://localhost:3000/oauth/fb";
	}
}
