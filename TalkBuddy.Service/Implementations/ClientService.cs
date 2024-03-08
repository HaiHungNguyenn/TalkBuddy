using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TalkBuddy.Common.Helpers;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;
using TalkBuddy.Service.Models.Common;

namespace TalkBuddy.Service.Implementations;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IEmailService _emailService;
    private readonly IOtpCodeRepository _otpCodeRepository;
    private readonly IUnitOfWork _unitOfWork;

	public ClientService(IClientRepository clientRepo, IUnitOfWork unitOfWork, IEmailService emailService, IOtpCodeRepository otpCodeRepository)
	{
		_clientRepository = clientRepo;
		_unitOfWork = unitOfWork;
		_emailService = emailService;
		_otpCodeRepository = otpCodeRepository;
	}


	public async Task<IQueryable<Client>> FindClient(string clientName)
    {
        return (await _clientRepository.GetAllAsync()).Where(c =>
            c.Name.Contains(clientName));
    }

	public async Task<Client?> LoginAsync(string username, string password)
	{
        var client = await _clientRepository.GetAsync(c => c.Email == username && c.IsVerified);
        if (client != null && PasswordHelper.IsValidPassword(password, client.Password))
        {
            return client;
        }

        return null;
	}

    public async Task<Client?> GetClientById(Guid clientId)
    {
        return await (await _clientRepository.FindAsync(x => x.Id == clientId))
            .Include(c => c.Friends)
            .Include(c => c.InformantClients)
            .Include(c => c.ReportedClients)
            .Include(c => c.InChatboxes)
            .Include(c => c.ClientMessages)
            .Include(c => c.CreatedChatBoxes)
            .FirstOrDefaultAsync();
    }
	public async Task RegisterAsync(string username, string password)
	{
		var client = await _clientRepository.GetAsync(c => c.Email == username);
        if (client != null && client.IsVerified)
            throw new Exception("User already exists");

        var code = new OtpCode
        {
            Code = RandomPasswordHelper.GenerateRandomPassword(10),
            Used = false,
            CreatedAt = DateTime.Now
        };
        
        if (client == null)
        {
            client = new Client
            {
                Name = username,
                Email = username,
                Password = PasswordHelper.HashPassword(password),
                Gender = "N/A",
                IsVerified = false,
            };

            client.Codes.Add(code);
            await _clientRepository.AddAsync(client);
        }
        else
        {
            client.Codes.Add(code);
            await _clientRepository.UpdateAsync(client);
        }

        await _unitOfWork.CommitAsync();

        var email = new OtpEmail
        {
            To = username,
            Otp = code.Code,
            Subject = "Confirm your email",
            Username = username,
            Body = File.ReadAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/wwwroot/template/otp-email.html")
        };

        _emailService.SendOptEmailAsync(email);
	}

	public async Task<Client> ConfirmCodeAsync(string code)
	{
        var otpCode = await _otpCodeRepository.GetAsync(c => c.Code == code && !c.Used) ?? throw new Exception("Otp code does not exist");
        var client = await _clientRepository.GetAsync(c => c.Id == otpCode.ClientId) ?? throw new Exception("Client not found");
        otpCode.Used = true;
        client.IsVerified = true;

        await _otpCodeRepository.UpdateAsync(otpCode);
        await _clientRepository.UpdateAsync(client);
        await _unitOfWork.CommitAsync();

        return client;
	}
}