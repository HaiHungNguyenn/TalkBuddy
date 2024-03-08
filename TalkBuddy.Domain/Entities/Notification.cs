using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class Notification : BaseEntity<Guid>
{
    public Guid ClientId { get; set; } 
    public string Message { get; set; }
    public Client Client { get; set; } = null!;
    
    public DateTime SendAt { get; set; }
    
    public bool IsRead { get; set; }
}