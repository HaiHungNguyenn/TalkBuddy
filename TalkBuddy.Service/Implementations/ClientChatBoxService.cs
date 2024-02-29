using Microsoft.EntityFrameworkCore;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations
{
    public class ClientChatBoxService : IClientChatBoxService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientChatBoxService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IList<ClientChatBox>> GetClientChatBoxes()
        {
            return await _unitOfWork.ClientChatBoxRepository.GetAll().ToListAsync();
        }

        public async Task<IList<ClientChatBox>> GetClientChatBoxesIncludeNotEmptyMessages(Guid clientId)
        {
            return await _unitOfWork.ClientChatBoxRepository.Find(x=>x.ClientId.Equals(clientId)).Include(x=>x.ChatBox).ThenInclude(x=>x.Messages).Where(b=>b.ChatBox.Messages.Any()).Include(x=>x.Client).ToListAsync();
        }

        public async Task<IList<ClientChatBox>> GetClientChatBoxes(Guid clientId)
        {
            return await _unitOfWork.ClientChatBoxRepository.Find(x => x.ClientId.Equals(clientId)).Include(x => x.ChatBox).Include(x => x.Client).ToListAsync();
        }

        public async Task<IList<ClientChatBox>> GetClientOfChatBoxes(Guid chatBoxId)
        {
            return await _unitOfWork.ClientChatBoxRepository.Find(x => x.ChatBoxId.Equals(chatBoxId)).Include(x => x.Client).ToListAsync();
        }
    }
}
