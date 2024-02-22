using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations;

public class ClientRepo : GenericRepository<Client>, IClientRepo
{
    public ClientRepo(TalkBuddyContext dbContext) : base(dbContext)
    {
    }
}