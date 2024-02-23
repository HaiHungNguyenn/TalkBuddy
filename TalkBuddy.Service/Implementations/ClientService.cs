using TalkBuddy.Common.Helpers;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    public ClientService(IClientRepository clientRepository) 
    {
        _clientRepository = clientRepository;
    }


    public async Task<IQueryable<Client>> FindClient(string clientName)
    {
        return (await _clientRepository.GetAllAsync()).Where(c =>
            c.Name.Contains(clientName));
    }

	public async Task<Client?> LoginAsync(string username, string password)
	{
        var client = await _clientRepository.GetAsync(c => c.Email == username);
        if (client != null && PasswordHelper.IsValidPassword(password, client.Password))
        {
            return client;
        }

        return null;
	}

    public async Task<Client> GetClientById(Guid clientId)
    {
        return await _clientRepository.GetAsync(x => x.Id == clientId) ?? throw new Exception("Not found client");
    }
}