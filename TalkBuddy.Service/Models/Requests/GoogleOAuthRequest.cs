namespace TalkBuddy.Service.Models.Requests
{
    public class GoogleOAuthRequest
    {
        public string State { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Scope { get; set; } = null!;

        public string Authuser { get; set; } = null!;

        public string Hd { get; set; } = null!;

        public string Prompt { get; set; } = null!;
    }
}
