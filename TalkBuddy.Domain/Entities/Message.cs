using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class Message : BaseEntity<Guid>
{
<<<<<<< HEAD
=======
    public string Content { get; set; }
    public DateTime SentDate { get; set; }
    
    public Guid ChatBoxId { get; set; }
    public virtual ChatBox ChatBox { get; set; } = null!;
    public virtual ICollection<Media> Medias { get; set; } = new List <Media>();
>>>>>>> 175e0874c05c9bc60e78922c840f0914757446ab
}