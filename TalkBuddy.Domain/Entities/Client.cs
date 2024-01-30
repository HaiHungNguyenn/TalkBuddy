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

    public List<TaggedClientMessage>? TaggedClientMessages { get; set; }
    public List<Friendship>? Friends { get; set; }
    public List<Report>? ReportedClients { get; set; }
    public List<Report>? InformantClients { get; set; }
    public List<ClientChatbox>? InChatboxes { get; set; }
    public List<ChatBox>? CreatedChatBoxes { get; set; }
    public List<ClientMessage>? ClientMessages { get; set; }



}