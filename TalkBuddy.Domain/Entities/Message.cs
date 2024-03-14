using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class Message : BaseEntity<Guid>
{
    public string Content { get; set; }
    public DateTime SentDate { get; set; }
    
    public Guid ChatBoxId { get; set; }
    public Guid SenderId { get; set; }
    public virtual Client Sender { get; set; } = null!;
    public virtual ChatBox ChatBox { get; set; } = null!;
    public virtual ICollection<Media> Medias { get; set; } = new List <Media>();
  
}