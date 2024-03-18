namespace TalkBuddy.Service.Settings
{
	public class FacebookSettings
	{
		public string ClientId { get; init; } = null!;
		public string ClientSecret { get; init; } = null!;
		public string RedirectUri { get; init; } = null!;
	}
}