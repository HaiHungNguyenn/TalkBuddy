
using TalkBuddy.Common.Enums;
using TalkBuddy.Domain.Entities;


namespace TalkBuddy.Service.Interfaces;

public interface IFriendShipService
{
    Task CreateFriendShip(Friendship friendship);

    Task<IQueryable<Friendship>> GetFriendInvitation(Guid receiverId);
    FriendShipRequestStatus? GetFriendShipStatus(Guid senderId, Guid receiverId);
    
    Task<FriendShipRequestStatus?> GetFriendShipStatusAsync(Guid senderId, Guid receiverId);
    Task AcceptFriendInvitation(Guid friendShipId);
    Task RejectFriendInvitation(Guid friendShipId);
    Task CancelInvitation(Guid senderId, Guid receiver);

}