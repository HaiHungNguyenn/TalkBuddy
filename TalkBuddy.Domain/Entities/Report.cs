using System.ComponentModel.DataAnnotations.Schema;
using TalkBuddy.Common.Enums;
using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public partial class Report : BaseEntity<Guid>
{
    public Guid ReportedClientId { get; set; }
    public Guid InformantClientId { get; set; }
    public DateTime CreatedDate { get; set; }
    public ReportationStatus Status { get; set; }
    public string? Details { get; set; }
    [ForeignKey("ReportedClientId")]
    public Client? ReportedClient { get; set; }
    [ForeignKey("InformantClientId")]
    public Client? InformantClient { get; set; }
    public ICollection<ReportEvidence> ReportEvidences { get; set; } = new List<ReportEvidence>();

}