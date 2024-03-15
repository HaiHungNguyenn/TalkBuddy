using TalkBuddy.Domain.Dtos;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Presentation.SignalR;

public interface INotificationHub
{
    Task HandleAccept(Guid friendshipId, Guid senderId, Guid receiverId);
    Task HandleReject(Guid friendshipId, Guid senderId, Guid receiverId);
    Task SendNotification(string receiverId, List<DtoNotification> notifications);
    Task UpdateNotificationStatus();
    Task HandleAddFriend(string friendId);
}