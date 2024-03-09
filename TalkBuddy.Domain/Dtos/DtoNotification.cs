namespace TalkBuddy.Domain.Dtos;

public class DtoNotification
{
    public string Message { get; set; }
    public Guid ClientId { get; set; } 
    public DateTime SendAt { get; set; }
    public bool IsRead { get; set; }
    
    public string? ClientAvatar { get; set; }
}