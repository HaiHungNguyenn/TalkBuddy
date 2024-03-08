namespace TalkBuddy.Service.Models.Common;

public class OptEmail
{
    public string To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Opt { get; set; } = null!;
    

    public string GetParsedBody()
    {
        return Body.Replace($"{{{nameof(Opt)}}}", Opt)
            .Replace($"{{{nameof(Username)}}}", Username);
    }
}