using TalkBuddy.Domain.Entities.BaseEntities;
using TalkBuddy.Domain.Enums;

namespace TalkBuddy.Domain.Entities;

public partial class Media : BaseEntity<Guid>
{
    public MediaType Type { get; set; }
    public float Capacity { get; set; }
    
    public Guid MessageId { get; set; }
    public virtual Message Message { get; set; } = null!;

}