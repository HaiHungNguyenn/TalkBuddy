using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations;

public class FriendShipRepository : GenericRepository<Friendship>, IFriendShipRepository
{
    public FriendShipRepository(TalkBuddyContext dbContext) : base(dbContext)
    {
    }
}