using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task CreateNotification(Notification notification)
    {
        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IQueryable<Notification>> GetNotificationByClient(Guid clientId)
    {
        var notifications =  await _notificationRepository.FindAsync(x => x.ClientId == clientId);
        return notifications;
    }

    public async Task UpdateNotificationStatus(IQueryable<Notification> notifications)
    {
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await _notificationRepository.UpdateManyAsync(notifications);
        await _unitOfWork.CommitAsync();
    }
}