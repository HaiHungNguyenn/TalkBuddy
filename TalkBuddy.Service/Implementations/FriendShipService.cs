using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations;

public class FriendShipService : IFriendShipService
{
    private readonly IFriendShipRepository _friendShipRepository;
    private readonly IUnitOfWork _unitOfWork;
    public FriendShipService(IFriendShipRepository friendShipRepository, IUnitOfWork unitOfWork)
    {
        _friendShipRepository = friendShipRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task CreateFriendShip(Friendship friendship)
    {
       await _friendShipRepository.AddAsync(friendship);
       await _unitOfWork.CommitAsync();
    }
}