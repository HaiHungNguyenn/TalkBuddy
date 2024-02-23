using TalkBuddy.Common.Enums;
using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class Friendship : BaseEntity<Guid>
{
    public Guid SenderID { get; set; }
    public Guid ReceiverId { get; set; }
    public FriendShipRequestStatus Status { get; set; } = FriendShipRequestStatus.WAITING;
    public DateTime RequestDate { get; set; }
    public DateTime AcceptDate { get; set; }

    public Client Sender { get; set; }
    public Client Receiver { get; set; }
}