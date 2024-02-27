namespace TalkBuddy.DAL.Interfaces;

public interface IUnitOfWork
{
    IChatBoxRepository ChatBoxRepository { get; }
    IMessageRepository MessageRepository { get; }
    IUserConnectionRepository UserConnectionRepository { get; }
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}