using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class Report : BaseEntity<Guid>
{
    public Guid ReportedClientId { get; set; }
    public Guid InformantClientId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Status { get; set; }
    public string? Details { get; set; }

    public Client ReportedClient { get; set; }
    public Client InformantClient { get; set; }

    public List<TaggedClientMessage> TaggedClientMessages { get; set; }
}