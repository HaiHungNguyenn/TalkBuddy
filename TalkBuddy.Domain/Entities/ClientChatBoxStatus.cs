using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public class ClientChatBoxStatus : BaseEntity<Guid>
{
    public Guid ClientChatBoxId { get; set; }
    public Guid MessageId { get; set; }
    public bool IsRead { get; set; }

    public ClientChatBox ClientChatBox { get; set; } = null!;
    public Message Message { get; set; } = null!;
}