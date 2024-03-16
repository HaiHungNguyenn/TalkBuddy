using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TalkBuddy.Common.Enums;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Domain.Enums;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations
{
    public class ClientChatBoxService : IClientChatBoxService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFriendShipRepository _friendShipRepository;
        private readonly IClientRepository _clientRepository;

        public ClientChatBoxService(IUnitOfWork unitOfWork, IFriendShipRepository friendShipRepository, IClientRepository clientRepository)
        {
            _unitOfWork = unitOfWork;
            _friendShipRepository = friendShipRepository;
            _clientRepository = clientRepository;
        }
        public async Task<IList<ClientChatBox>> GetClientChatBoxes()
        {
            return await _unitOfWork.ClientChatBoxRepository.GetAll().Include(clb => clb.ChatBox).OrderByDescending(clb => clb.ChatBox.CreatedDate).ToListAsync();
        }

        public async Task<IList<ClientChatBox>> GetClientChatBoxesIncludeNotEmptyMessages(Guid clientId)
        {
            return await _unitOfWork.ClientChatBoxRepository.Find(x => x.ClientId.Equals(clientId)).Include(x => x.ChatBox).ThenInclude(x => x.Messages).Where(b => b.ChatBox.Messages.Any()).Include(x => x.Client).ToListAsync();
        }

        public async Task<IList<ClientChatBox>> GetClientChatBoxes(Guid clientId)
        {

            var res = await _unitOfWork.ClientChatBoxRepository.FindAsync(x => x.ClientId.Equals(clientId));
            return res.Include(x => x.ChatBox)
                .Where(x => (x.IsLeft && x.ChatBox.Type == ChatBoxType.TwoPerson) || !x.IsLeft)
                .OrderByDescending(clb => clb.ChatBox.CreatedDate).Include(x => x.Client).ToList();
        }

        public async Task<IList<ClientChatBox>> GetClientOfChatBoxes(Guid chatBoxId)
        {
            var res = await _unitOfWork.ClientChatBoxRepository.FindAsync(x => x.ChatBoxId.Equals(chatBoxId)&&!x.IsLeft);
            return res.Include(x => x.Client).ToList();
        }

        public async Task<IList<ClientChatBox>> GetClientOfChatBoxesOfAUserBySearchName(string searchName, Guid userId)
        {
            return await _unitOfWork.ClientChatBoxRepository.Find(x => x.ClientId.Equals(userId))
                .Include(x => x.Client)
                .Include(x => x.ChatBox).ThenInclude(c => c.ClientChatBoxes)
                .Where(c => c.ChatBox.ChatBoxName
                .Contains(searchName) ||
                c.ChatBox.ClientChatBoxes
                .Any(c => !c.ClientId.Equals(userId) && c.NickName.Contains(searchName))).ToListAsync();
        }

        public async Task<string> GetChatBoxNameOfTwoPersonType(Guid chatBoxId, Guid userId)
        {
            return await _unitOfWork.ClientChatBoxRepository.
                Find(x => x.ChatBoxId.Equals(chatBoxId) && !x.ClientId.Equals(userId))
                .Select(x => x.NickName).FirstOrDefaultAsync();
        }

        public async Task RemoveClientFromChatBox(Guid chatBoxId, Guid clientId)
        {
            var clientChatBox = await _unitOfWork.ClientChatBoxRepository.FindAsync(x => x.ChatBoxId.Equals(chatBoxId) && x.ClientId.Equals(clientId));
            var updateClientChatBox = clientChatBox.FirstOrDefault();
            updateClientChatBox.IsLeft = true;
            await _unitOfWork.ClientChatBoxRepository.UpdateAsync(updateClientChatBox);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IList<Client>> GetFriendsNotInChatBoxes(Guid chatBoxId, Guid userId)
        {
            var friendships = await (await _friendShipRepository.GetAllAsync())
            .Where(fs => fs.Status == FriendShipRequestStatus.ACCEPTED && (fs.SenderID == userId || fs.ReceiverId == userId))
            .Include(fs => fs.Sender)
            .Include(fs => fs.Receiver)
            .ToListAsync();

             var friends = friendships.Select(fs => fs.SenderID == userId ? fs.Receiver : fs.Sender);
            IList<Client> returnList = new List<Client>();
            foreach (var friend in friends)
            {
                var list = await _unitOfWork.ClientChatBoxRepository
                    .Find(x => x.ChatBoxId.Equals(chatBoxId) && x.ClientId.Equals(friend.Id)&&!x.IsLeft).ToListAsync();
                if (list.IsNullOrEmpty())
                {
                    returnList.Add(friend);
                }
            }
            return returnList;
        }

        public async Task AddClientToGroup(ClientChatBox clientChatBox)
        {
            var x= await _unitOfWork.ClientChatBoxRepository.FindAsync(x=>x.ChatBoxId.Equals(clientChatBox.ChatBoxId)&&x.ClientId.Equals(clientChatBox.ClientId));
            if (x.Any())
            {
                var updateClientChatBox = x.FirstOrDefault();
                updateClientChatBox.IsLeft = false;
                await _unitOfWork.ClientChatBoxRepository.UpdateAsync(updateClientChatBox);
                await _unitOfWork.CommitAsync();
                return;
            }
            await _unitOfWork.ClientChatBoxRepository.AddAsync(clientChatBox);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IList<ClientChatBox>> GetClientOfChatBoxes(Guid chatBoxId, Guid userId)
        {
            return await _unitOfWork.ClientChatBoxRepository.Find(x => x.ChatBoxId.Equals(chatBoxId) && x.ClientId.Equals(userId)).Include(x => x.Client).ToListAsync();
        }

        public async Task Update(ClientChatBox clientChatBox)
        {
            await _unitOfWork.ClientChatBoxRepository.UpdateAsync(clientChatBox);
            await _unitOfWork.CommitAsync();
        }
    }
}
