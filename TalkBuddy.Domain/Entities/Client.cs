using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class Client: BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;
    public DateTime LastLoginDate { get; set; }
    public string ProfilePicture { get; set; }

    public ICollection<TaggedClientMessage>? TaggedClientMessages { get; set; }
    public ICollection<Friendship>? Friends { get; set; }
    public ICollection<Report>? ReportedClients { get; set; }
    public ICollection<Report>? InformantClients { get; set; }
    public ICollection<ClientChatBox>? InChatboxes { get; set; }
    public ICollection<ChatBox>? CreatedChatBoxes { get; set; }
    public ICollection<ClientMessage>? ClientMessages { get; set; }

}