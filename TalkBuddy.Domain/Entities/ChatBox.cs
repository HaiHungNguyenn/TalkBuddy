using TalkBuddy.Domain.Entities.BaseEntities;
using TalkBuddy.Domain.Enums;

namespace TalkBuddy.Domain.Entities;

public partial class ChatBox : BaseEntity<Guid>
{
    public string ChatBoxName { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public string? ChatBoxAvatar { get; set; }
    public ChatBoxType Type { get; set; }

    
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public Guid GroupCreatorId { get; set; }
    public virtual Client GroupCreator { get; set; } = null!;
    public virtual ICollection<ClientChatBox> ClientChatBoxes { get; set; } = new List<ClientChatBox>();
}