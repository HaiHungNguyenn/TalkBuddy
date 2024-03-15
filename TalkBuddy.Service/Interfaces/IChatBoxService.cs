using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces
{
    public interface IChatBoxService
    {
        Task<ChatBox> GetChatBoxAsync(Guid chatBoxId);
        Task CreateNewChatBox(ChatBox chatBox);
        Task UpdateChatBox(ChatBox chatBox);
    }
}