
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TalkBuddy.Common.Enums;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Domain.Enums;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations;

public class FriendShipService : IFriendShipService
{
    private readonly IFriendShipRepository _friendShipRepository;
    private readonly IUnitOfWork _unitOfWork;
	private readonly IClientRepository _clientRepository;
	private readonly IChatBoxRepository _chatBoxRepository;
    private readonly IClientChatBoxService _clientChatboxService;

    public FriendShipService(IFriendShipRepository friendShipRepository,
                             IUnitOfWork unitOfWork,
                             IClientRepository clientRepository,
                             IChatBoxRepository chatBoxRepository,
                             IClientChatBoxService clientChatboxService)
    {
        _friendShipRepository = friendShipRepository;
		_clientRepository = clientRepository;
		_chatBoxRepository = chatBoxRepository;
		_unitOfWork = unitOfWork;
        _clientChatboxService = clientChatboxService;
    }

    public async Task CreateFriendShip(Friendship friendShip)
    {
        var friendShipStatus = await GetFriendShipStatusAsync(friendShip.SenderID, friendShip.ReceiverId);
        if (friendShipStatus == null)
        {
            await _friendShipRepository.AddAsync(friendShip);
            await _unitOfWork.CommitAsync();
        }
        else if (friendShipStatus.Equals(FriendShipRequestStatus.REJECTED) || friendShipStatus.Equals(FriendShipRequestStatus.CANCEL))
        {
            var x = await _friendShipRepository.GetAsync(x =>
                (x.SenderID == friendShip.SenderID && x.ReceiverId == friendShip.ReceiverId)||
                (x.SenderID == friendShip.ReceiverId && x.ReceiverId == friendShip.SenderID));
            x.Status = FriendShipRequestStatus.WAITING;
            x.SenderID = friendShip.SenderID;
            x.ReceiverId = friendShip.ReceiverId;
            await _friendShipRepository.UpdateAsync(x);
            await _unitOfWork.CommitAsync();
        }
    }
    
    public FriendShipRequestStatus? GetFriendShipStatus(Guid senderId, Guid receiverId)
    {
        var x =  _friendShipRepository.Get(x =>
            (x.SenderID == senderId && x.ReceiverId == receiverId)||
            (x.SenderID == receiverId && x.ReceiverId == senderId));
        return x?.Status;
    }

    public async Task<FriendShipRequestStatus?> GetFriendShipStatusAsync(Guid senderId, Guid receiverId)
    {
        var x = await _friendShipRepository.GetAsync(x =>
            (x.SenderID == senderId && x.ReceiverId == receiverId)||
            (x.SenderID == receiverId && x.ReceiverId == senderId));
        return x?.Status;
    }

    public async Task<IQueryable<Friendship>> GetFriendInvitation(Guid receiverId)
    {
        var listFriend = (await _friendShipRepository.FindAsync(x => x.ReceiverId == receiverId))
            .Include(fs => fs.Sender);
        var listInvitation = listFriend.Where(friendship => friendship.Status == FriendShipRequestStatus.WAITING);
        return listInvitation;
    }

    
    public async Task AcceptFriendInvitation(Guid friendShipId)
    {
        var friendship = await (await _friendShipRepository.FindAsync(x => x.Id == friendShipId))
	        .Include(fs => fs.Sender)
	        .Include(fs => fs.Receiver)
	        .FirstOrDefaultAsync() ?? throw new Exception("Not Found FriendInvitation");
		if (friendship.Status != FriendShipRequestStatus.WAITING) return;
			
		var chatbox = new ChatBox
		{
			ChatBoxName = $"{friendship.Sender.Name} - {friendship.Receiver.Name}",
			CreatedDate = DateTime.Now,
			Type = ChatBoxType.TwoPerson,
			GroupCreatorId = friendship.SenderID
		};

		chatbox.ClientChatBoxes.Add(new ClientChatBox
		{
			ClientId = friendship.SenderID,
			ChatBox = chatbox,
			IsBlocked = false,
			IsLeft = false,
			IsNotificationOn = true,
			IsModerator = true,
			NickName = friendship.Sender.Name
		});

		chatbox.ClientChatBoxes.Add(new ClientChatBox
		{
			ClientId = friendship.ReceiverId,
			ChatBox = chatbox,
			IsBlocked = false,
			IsLeft = false,
			IsNotificationOn = true,
			IsModerator = true,
			NickName = friendship.Receiver.Name
		});

		await _chatBoxRepository.AddAsync(chatbox);
		friendship.Status = FriendShipRequestStatus.ACCEPTED;
		await _friendShipRepository.UpdateAsync(friendship);
        await _unitOfWork.CommitAsync();
    }
    public async Task RejectFriendInvitation(Guid friendShipId)
    {
        var friendship = await _friendShipRepository.GetAsync(x => x.Id == friendShipId) ?? throw new Exception("Not Found FriendInvitation");
        friendship.Status = FriendShipRequestStatus.REJECTED;
        await _friendShipRepository.UpdateAsync(friendship);
        await _unitOfWork.CommitAsync();
    }

    public async Task CancelInvitation(Guid senderId, Guid receiverId)
    {
        var friendship = await _friendShipRepository.GetAsync(x =>
               (x.SenderID == senderId && x.ReceiverId == receiverId) ||
               (x.SenderID == receiverId && x.ReceiverId == senderId));

		if (friendship == null || friendship.Status != FriendShipRequestStatus.WAITING) return;

        friendship.Status = FriendShipRequestStatus.CANCEL;
        await _friendShipRepository.UpdateAsync(friendship);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<Client>> GetClientFriends(Guid clientId)
    {
        var friendships = await (await _friendShipRepository.GetAllAsync())
            .Where(fs => fs.Status == FriendShipRequestStatus.ACCEPTED && (fs.SenderID == clientId || fs.ReceiverId == clientId))
            .Include(fs => fs.Sender)
            .Include(fs => fs.Receiver)
            .ToListAsync();

        return friendships.Select(fs => fs.SenderID == clientId ? fs.Receiver : fs.Sender);
    }

    public async Task DeleteFriendShip(Guid friendId, Guid clientId)
    {
        var x = await _friendShipRepository.GetAsync(x =>
               (x.SenderID == friendId && x.ReceiverId == clientId) ||
               (x.SenderID == clientId && x.ReceiverId == friendId)) ?? throw new Exception("Friendship does not exist");

        var senderChatboxes = await _clientChatboxService.GetClientChatBoxes(x.SenderID);
        var receiverChatboxes = await _clientChatboxService.GetClientChatBoxes(x.ReceiverId);

        var commonChatboxIds = GetCommonChatboxIds(senderChatboxes, receiverChatboxes);

        var chatbox = await _chatBoxRepository.GetAsync(c => commonChatboxIds.Contains(c.Id) && c.Type == ChatBoxType.TwoPerson);
        if (chatbox != null)
        {
            var senderChatbox = chatbox.ClientChatBoxes.FirstOrDefault(c => c.ClientId == x.SenderID);
            var receiverChatbox = chatbox.ClientChatBoxes.FirstOrDefault(c => c.ClientId == x.ReceiverId);
            if (senderChatbox != null)
                await _clientChatboxService.RemoveClientFromChatBox(senderChatbox.ChatBoxId, x.SenderID);
            if (receiverChatbox != null)
                await _clientChatboxService.RemoveClientFromChatBox(receiverChatbox.ChatBoxId, x.ReceiverId);
        }

        x.Status = FriendShipRequestStatus.REJECTED;
        await _friendShipRepository.UpdateAsync(x);
        await _unitOfWork.CommitAsync();
    }

    public async Task CreateFriendship(Guid clientId, Guid friendId)
    {
		var sender = await _clientRepository.GetAsync(c => c.Id == clientId) ?? throw new Exception("Client not found");
		var receiver = await _clientRepository.GetAsync(c => c.Id == friendId) ?? throw new Exception("Receiver not found");
        var friendship = await _friendShipRepository.GetAsync(x =>
               (x.SenderID == friendId && x.ReceiverId == clientId) ||
               (x.SenderID == clientId && x.ReceiverId == friendId));

		if (friendship == null)
		{
			friendship = new Friendship
			{
				SenderID = clientId,
				ReceiverId = friendId,
				Status = FriendShipRequestStatus.WAITING,
				RequestDate = DateTime.Now
			};
            
			await _friendShipRepository.AddAsync(friendship);
		}
		else if (friendship.Status == FriendShipRequestStatus.WAITING)
			return;
		else 
		{
			friendship.Status = FriendShipRequestStatus.WAITING;
			friendship.RequestDate = DateTime.Now;
			friendship.SenderID = clientId;
			friendship.ReceiverId = friendId;

			await _friendShipRepository.UpdateAsync(friendship);
		}

		await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<Client>> GetClientFriendsSearchByName(Guid clientId, string search)
    {
        var clientFriends = await GetClientFriends(clientId);

        // Filter the client friends based on the search string
        var filteredFriends = clientFriends.Where(client => client.Name.Contains(search));

        return filteredFriends;
    }

    private IList<Guid> GetCommonChatboxIds(IList<ClientChatBox> firstList, IList<ClientChatBox> secondList)
    {
        var results = new List<Guid>();

        foreach (var clientChatBox1 in firstList)
        {
            foreach (var clientChatBox2 in secondList)
            {
                if (clientChatBox1.ChatBoxId == clientChatBox2.ChatBoxId)
                    results.Add(clientChatBox1.ChatBoxId);
            }
        }

        return results;
    }
}
