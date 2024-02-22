namespace TalkBuddy.Domain.Dtos;

public class DtoClientForFriend
{
    public Guid id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool isFriend { get; set; } = false;
    
    public string? ProfilePicture { get; set; }
}