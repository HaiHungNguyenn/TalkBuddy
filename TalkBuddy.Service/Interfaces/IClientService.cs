using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces;

public interface IClientService
{
    Task<IQueryable<Client>> FindClient(string clientId);
}