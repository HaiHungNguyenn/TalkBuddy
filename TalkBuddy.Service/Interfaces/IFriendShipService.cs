using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces;

public interface IFriendShipService
{
    Task CreateFriendShip(Friendship friendship);
}