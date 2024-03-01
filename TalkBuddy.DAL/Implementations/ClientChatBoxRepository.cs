using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations
{
    public class ClientChatBoxRepository : GenericRepository<ClientChatBox>, IClientChatBoxRepository
    {
        public ClientChatBoxRepository(TalkBuddyContext dbContext) : base(dbContext)
        {
        }
    }
}
