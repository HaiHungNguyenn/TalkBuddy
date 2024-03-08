using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities;

public class OtpCode : BaseEntity<Guid>
{
    public Guid ClientId { get; set; }
    public string Code { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool Used { get; set; }
    public Client Client { get; set; } = null!;
}