using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations;

public class ClientChatBoxStatusRepository : GenericRepository<ClientChatBoxStatus>, IClientChatBoxStatusRepository
{
    public ClientChatBoxStatusRepository(TalkBuddyContext dbContext) : base(dbContext)
    {
    }
}