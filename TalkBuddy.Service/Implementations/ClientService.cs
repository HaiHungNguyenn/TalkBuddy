using TalkBuddy.Common.Helpers;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClientService(IClientRepository clientRepo, IUnitOfWork unitOfWork) 
    {
        _clientRepository = clientRepo;
        _unitOfWork = unitOfWork;
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
	public async Task<Client> RegisterAsync(string username, string password)
	{
		var client = await _clientRepository.GetAsync(c => c.Email == username);
        if (client != null)
            throw new Exception("User already exists");

        client = new Client
        {
            Name = username,
            Email = username,
            Password = PasswordHelper.HashPassword(password),
            Gender = "N/A"
        };
        
        await _clientRepository.AddAsync(client);
        await _unitOfWork.CommitAsync();

        return client;
	}

}