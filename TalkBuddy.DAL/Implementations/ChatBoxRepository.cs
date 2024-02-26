using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations
{
    public class ChatBoxRepository : GenericRepository<ChatBox>, IChatBoxRepository
    {
        public ChatBoxRepository(TalkBuddyContext dbContext) : base(dbContext)
        {
        }
    }
}
