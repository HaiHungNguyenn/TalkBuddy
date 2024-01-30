using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class TaggedClientMessage : BaseEntity<Guid>
{
    public Guid TaggedClientId { get; set; }
    public Guid MessageId { get; set; }

    public Client Client { get; set; }
    public Message Message { get; set; }
}