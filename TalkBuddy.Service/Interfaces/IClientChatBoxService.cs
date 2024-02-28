using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces
{
    public interface IClientChatBoxService
    {
        Task<IList<ClientChatBox>> GetClientChatBoxes();
        Task<IList<ClientChatBox>> GetClientChatBoxes(Guid clientId);
        Task<IList<ClientChatBox>> GetClientOfChatBoxes(Guid chatBoxId);
    }
}
