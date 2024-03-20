namespace TalkBuddy.Domain.Dtos;

public class ClientChatBoxWithSeenMessageDto
{
    public bool IsBlocked { get; set; }
    public bool IsLeft { get; set; }
    public bool IsNotificationOn { get; set; }
    public bool IsModerator { get; set; }
    public Guid ClientId { get; set; }
    public Guid ChatBoxId { get; set; }
    public string ChatBoxName { get; set; }
    public string ChatBoxAvatar { get; set; }
    public string ChatBoxType { get; set; }
    public int UnreadMessageCount { get; set; }
}