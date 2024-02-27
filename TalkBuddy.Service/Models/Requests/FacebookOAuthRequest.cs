namespace TalkBuddy.Service.Models.Requests
{
	public class FacebookOAuthRequest
	{
		public string AccessToken { get; set; } = null!;

		public long DataAccessExpirationTime { get; set; }

		public int ExpiresIn { get; set; }
	}
}
