using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces;

public interface IClientService
{
    Task<IQueryable<Client>> FindClient(string username);
    Task<Client?> LoginAsync(string username, string password);

    Task<Client> GetClientById(Guid clientId);
}