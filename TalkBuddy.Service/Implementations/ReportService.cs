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

    public async Task<IQueryable> GetSuspendedClient()
    {
        var clients =(await _clientRepository.FindAsync(x => x.IsAccountSuspended == true && x.SuspensionEndDate < DateTime.Now))
                .Include(x => x.InformantClients)
                .Include(x => x.ReportedClients);
        return clients;
    }
}