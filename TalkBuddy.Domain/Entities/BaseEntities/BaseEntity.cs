namespace TalkBuddy.Domain.Entities.BaseEntities;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; }
    public int TimeStamp { get; set; }
}