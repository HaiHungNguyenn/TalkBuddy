using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public class ReportEvidence : BaseEntity<Guid>
{
    public string EvidenceUrl { get; set; }
    public Guid ReportId { get; set; }
    public virtual Report Report { get; set; } = null!;
}