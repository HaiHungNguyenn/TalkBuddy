using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;

namespace TalkBuddy.DAL.Implementations;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly TalkBuddyContext _dbContext;

    public UnitOfWork(TalkBuddyContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IMessageRepository MessageRepository { get; set; }

    public IChatBoxRepository ChatBoxRepository { get; set; }

    public IClientChatBoxRepository ClientChatBoxRepository => new ClientChatBoxRepository(_dbContext);

    public void Commit()
    {
        _dbContext.SaveChanges();
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        try
        {
            GC.SuppressFinalize(this);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void Rollback()
    {
        Console.WriteLine("Transaction rollback");
    }

    public async Task RollbackAsync()
    {
        Console.WriteLine("Transaction rollback");
        await Task.CompletedTask;	
    }
}
