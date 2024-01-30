using System.ComponentModel.DataAnnotations.Schema;
using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class Report : BaseEntity<Guid>
{
    public Guid ReportedClientId { get; set; }
    public Guid InformantClientId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Status { get; set; }
    public string? Details { get; set; }
    [ForeignKey("ReportedClientId")]
    public Client ReportedClient { get; set; }
    [ForeignKey("InformantClientId")]
    public Client InformantClient { get; set; }

    public ICollection<TaggedClientMessage> TaggedClientMessages { get; set; }
}