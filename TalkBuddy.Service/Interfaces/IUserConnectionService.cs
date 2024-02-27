using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces
{
    public interface IUserConnectionService
    {
        Task AddConnection(UserConnection userConnection);
        Task RemoveConnection(UserConnection userConnection);
        Task<UserConnection> GetConnection(string connectionId, string userName);
        Task<UserConnection> GetConnection(string connectionId);
    }
}
