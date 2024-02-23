using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations;

public class ClientRepository : GenericRepository<Client>, IClientRepository
{
    public ClientRepository(TalkBuddyContext dbContext) : base(dbContext)
    {
    }
}