using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations;

public class ClientService : IClientService
{
    private readonly IClientRepo _clientRepo;
    public ClientService(IClientRepo clientRepo) 
    {
        _clientRepo = clientRepo;
    }


    public async Task<IQueryable<Client>> FindClient(string clientName)
    {
        return (await _clientRepo.GetAllAsync()).Where(c =>
            c.Name.Contains(clientName) || c.Email.Contains(clientName));
    }
}