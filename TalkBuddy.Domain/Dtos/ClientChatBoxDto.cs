

namespace TalkBuddy.Domain.Dtos
{
    public class ClientChatBoxDto
    {
        public bool IsBlocked { get; set; }
        public bool IsLeft { get; set; }
        public bool IsNotificationOn { get; set; }
        public bool IsModerator { get; set; }
        public Guid ClientId { get; set; }
        public Guid ChatBoxId { get; set; }
        public string ChatBoxName { get; set; }
        public string ChatBoxAvatar { get; set; }
    }
}
