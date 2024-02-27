using Microsoft.EntityFrameworkCore;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations
{

    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddMessage(Message message)
        {
            await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IList<Message>> GetMessages(Guid chatBoxId)
        {
            return await _unitOfWork.MessageRepository.Find(x=>x.ChatBoxId.Equals(chatBoxId)).ToListAsync();
        }
    }
}
