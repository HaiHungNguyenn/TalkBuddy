using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces
{
    public interface IMessageService
    {
        Task AddMessage(Message message);
        Task<IList<Message>> GetMessages(Guid chatBoxId);
    }
}
