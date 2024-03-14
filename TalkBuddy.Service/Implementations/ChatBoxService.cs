using Microsoft.EntityFrameworkCore;
using TalkBuddy.DAL.Implementations;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Domain.Enums;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations
{
    public class ChatBoxService : IChatBoxService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatBoxService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewChatBox(ChatBox chatBox)
        {         
            await _unitOfWork.ChatBoxRepository.AddAsync(chatBox);
            _unitOfWork.CommitAsync();
        }

        public async Task<ChatBox> GetChatBoxAsync(Guid chatBoxId)
        {
            return _unitOfWork.ChatBoxRepository.Find(x => x.Id == chatBoxId).Include(x => x.Messages).FirstOrDefault();  
        }
    }
}
