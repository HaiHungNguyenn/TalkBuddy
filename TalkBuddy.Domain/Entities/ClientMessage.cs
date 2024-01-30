using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class ClientMessage :  BaseEntity<Guid>
{
    public DateTime SeenDateTime { get; set; }
    public Guid ClientId { get; set; }
    public Guid MessageId { get; set; }
    public Message Message { get; set; } = null!;
    public Client Client { get; set; } = null!;
}