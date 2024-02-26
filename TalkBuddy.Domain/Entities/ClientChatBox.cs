using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class ClientChatBox : BaseEntity<Guid>
{
    public bool IsBlocked { get; set; }
    public bool IsLeft { get; set; }
    public bool IsNotificationOn { get; set; }
    public bool IsModerator { get; set; }
    public bool IsOnline { get; set; }
    
    public Guid ClientId { get; set; }
    public Guid ChatBoxId { get; set; }
    public virtual ChatBox ChatBox { get; set; } = null!;
    public virtual Client Client { get; set; } = null!;
}