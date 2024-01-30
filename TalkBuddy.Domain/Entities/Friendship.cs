using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class Friendship : BaseEntity<Guid>
{
    public Guid ClientId1 { get; set; }
    public Guid ClientId2 { get; set; }
    public bool Status { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime AcceptDate { get; set; }

    public Client Client1 { get; set; }
    public Client Client2 { get; set; }
}