using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations;

public class NotificationRepository : GenericRepository<Notification>,INotificationRepository
{
    public NotificationRepository(TalkBuddyContext dbContext) : base(dbContext)
    {
    }
}