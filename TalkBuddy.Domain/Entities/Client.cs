using System.Collections;
using TalkBuddy.Common.Enums;
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
    public string? ProfilePicture { get; set; }
    public bool IsVerified { get; set; }
    
    public bool IsAccountSuspended { get; set; }
    
    public int SuspensionCount { get; set; }
    
    public DateTime SuspensionEndDate { get; set; }

    public UserRole Role { get; set; } = UserRole.CLIENT;
    public ICollection<Friendship> Friends { get; set; } = new List<Friendship>();
    public ICollection<Report>? ReportedClients { get; set; }
    public ICollection<Report>? InformantClients { get; set; }
    public ICollection<ClientChatBox>? InChatboxes { get; set; }
    public ICollection<ChatBox>? CreatedChatBoxes { get; set; }
    public ICollection<ClientMessage>? ClientMessages { get; set; }
    public ICollection<Message>? Messages { get; set; }    
    public ICollection<Notification>? Notifications { get; set; }
    public ICollection<OtpCode> Codes { get; set; } = new List<OtpCode>();

}