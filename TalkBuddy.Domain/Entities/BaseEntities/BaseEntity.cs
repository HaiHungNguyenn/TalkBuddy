using System.ComponentModel.DataAnnotations;

namespace TalkBuddy.Domain.Entities.BaseEntities;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; }

    [ConcurrencyCheck]
    public int Version { get; set; }
}