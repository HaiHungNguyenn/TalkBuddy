using TalkBuddy.Domain.Entities.BaseEntities;
using TalkBuddy.Domain.Enums;

namespace TalkBuddy.Domain.Entities;

public partial class ChatBox : BaseEntity<Guid>
{
<<<<<<< HEAD

=======
    public string ChatBoxName { get; set; } 
    public DateTime CreatedDate { get; set; }
    public ChatBoxType Type { get; set; }
>>>>>>> 175e0874c05c9bc60e78922c840f0914757446ab
    
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public Guid GroupCreatorId { get; set; }
    public virtual Client GroupCreator { get; set; } = null!;
    public virtual ICollection<ClientChatBox> ClientChatBoxes { get; set; } = new List<ClientChatBox>();
}