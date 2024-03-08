namespace TalkBuddy.Domain.Dtos;

public class DtoClient
{
    public Guid id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? ProfilePicture { get; set; }
}