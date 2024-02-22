using TalkBuddy.Common.Helpers;
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

	public async Task<Client?> LoginAsync(string username, string password)
	{
        var client = await _clientRepo.GetAsync(c => c.Email == username);
        if (client != null && PasswordHelper.IsValidPassword(password, client.Password))
        {
            return client;
        }

        return null;
	}
}