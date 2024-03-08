namespace TalkBuddy.Service.Models.Common;

public class OtpEmail
{
    public string To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Otp { get; set; } = null!;
    

    public string GetParsedBody()
    {
        return Body.Replace($"{{{nameof(Otp)}}}", Otp)
            .Replace($"{{{nameof(Username)}}}", Username);
    }
}