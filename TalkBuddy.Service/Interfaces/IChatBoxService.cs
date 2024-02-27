using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces
{
    public interface IChatBoxService
    {
        Task<ChatBox> GetChatBoxAsync(Guid chatBoxId);
    }
}
