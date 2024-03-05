using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces;

public interface INotificationService
{
    Task CreateNotification(Notification notification);
    Task<IQueryable<Notification>> GetNotificationByClient(Guid clientId);
}