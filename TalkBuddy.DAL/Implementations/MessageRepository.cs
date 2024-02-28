using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(TalkBuddyContext dbContext) : base(dbContext)
        {
        }
    }
}
