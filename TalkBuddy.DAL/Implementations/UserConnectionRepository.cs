using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations
{
    public class UserConnectionRepository : GenericRepository<UserConnection>, IUserConnectionRepository
    {
        public UserConnectionRepository(TalkBuddyContext dbContext) : base(dbContext)
        {
        }
    }
}
