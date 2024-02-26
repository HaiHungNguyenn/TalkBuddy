using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
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
        public async Task<ChatBox> GetChatBoxAsync(Guid chatBoxId)
        {
            return _unitOfWork.ChatBoxRepository.FindAsync(x=>x.Id == chatBoxId).Result.FirstOrDefault();   
        }
    }
}
