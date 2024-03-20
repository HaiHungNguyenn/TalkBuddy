using Microsoft.EntityFrameworkCore;
using TalkBuddy.Common.Enums;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientRepository _clientRepository;

    public ReportService(IReportRepository reportRepository,IUnitOfWork unitOfWork, IClientRepository clientRepository)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
    }
    
    public async Task CreateReport(Report report)
    {
        await _reportRepository.AddAsync(report);
        await _unitOfWork.CommitAsync();
    }

    public async Task DismissReport(Guid reportId)
    {
        var report = await _reportRepository.GetAsync(x => x.Id == reportId) ?? throw new Exception($"Not Found Report: {reportId}");
        report.Status = ReportationStatus.DISMISSED;
        await _reportRepository.UpdateAsync(report);
        await _unitOfWork.CommitAsync();
    }

    public async Task BanUser(Guid reportId, SuspensionType suspensionType)
    {
        var report = await _reportRepository.GetAsync(x => x.Id == reportId) ?? throw new Exception($"Not Found Report: {reportId}");
        report.Status = ReportationStatus.RESOLVED;
        var client = await _clientRepository.GetAsync(x => x.Id == report.ReportedClientId);
        client.IsAccountSuspended = true;
        client.SuspensionCount++;
        client.SuspensionEndDate = suspensionType switch
        {
            SuspensionType.SIXHOUR => client.SuspensionEndDate.AddHours(6),
            SuspensionType.ONEDAY => client.SuspensionEndDate.AddDays(1),
            SuspensionType.THREEDAY => client.SuspensionEndDate.AddDays(3),
            SuspensionType.ONEMONTH => client.SuspensionEndDate.AddMonths(1),
            SuspensionType.SIXMONTH => client.SuspensionEndDate.AddMonths(6),
            _ => client.SuspensionEndDate 
        };
        await _clientRepository.UpdateAsync(client);
        await _reportRepository.UpdateAsync(report);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IQueryable<Client>> GetSuspendedClient()
    {
        var clients =(await _clientRepository.FindAsync(x => x.IsAccountSuspended == true && x.SuspensionEndDate >= DateTime.Now))
                .Include(x => x.InformantClients)
                .Include(x => x.ReportedClients);
        return clients;
    }

    public async Task<IEnumerable<Report>> GetAllReports()
    {
        return await (await _reportRepository.GetAllAsync()).Include(r => r.ReportedClient).ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetWaitingReports()
    {
        return await (await _reportRepository.GetAllAsync())
            .Where(r => r.Status == ReportationStatus.WAITING)
            .Include(r => r.ReportedClient)
            .ToListAsync();
    }

    public async Task<Report> GetReportById(Guid reportId)
    {
        return await (await _reportRepository.FindAsync(r => r.Id == reportId))
            .Include(r => r.ReportedClient)
            .Include(r => r.InformantClient)
            .Include(r => r.ReportEvidences)
            .FirstOrDefaultAsync() ?? throw new Exception("Report not found");
    }

    public async Task SuspendAccount(Guid reportId)
    {
        var report = await _reportRepository.GetAsync(r => r.Id == reportId) ?? throw new Exception("Report not found");
        var reportedClient = await _clientRepository.GetAsync(c => c.Id == report.ReportedClientId) ?? throw new Exception("Reported client not found");

        reportedClient.IsAccountSuspended = true;
        reportedClient.SuspensionCount++;

        reportedClient.SuspensionEndDate = reportedClient.SuspensionCount switch
        {
            1 => DateTime.Now.AddHours(7).AddHours(6),
            2 => DateTime.Now.AddHours(7).AddDays(1),
            3 => DateTime.Now.AddHours(7).AddDays(3),
            4 => DateTime.Now.AddHours(7).AddMonths(1),
            5 => DateTime.Now.AddHours(7).AddMonths(6),
            _ => DateTime.Now.AddHours(7).AddMonths(6)
        };

        report.Status = ReportationStatus.RESOLVED;
        
        await _clientRepository.UpdateAsync(reportedClient);
        await _reportRepository.UpdateAsync(report);
        await _unitOfWork.CommitAsync();
    }

    public async Task UnbanUser(Guid userId)
    {
        var user = await _clientRepository.GetAsync(u => u.IsAccountSuspended && u.Id == userId) ?? throw new Exception("User not found");
        user.IsAccountSuspended = false;
        
        await _clientRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();
    }
}