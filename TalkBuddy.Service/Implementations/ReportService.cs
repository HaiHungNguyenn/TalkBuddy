using TalkBuddy.Common.Enums;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IReportRepository reportRepository,IUnitOfWork unitOfWork)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
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

    public async Task BanUser(Guid reportId)
    {
        var report = await _reportRepository.GetAsync(x => x.Id == reportId) ?? throw new Exception($"Not Found Report: {reportId}");
        report.Status = ReportationStatus.RESOLVED;
        await _reportRepository.UpdateAsync(report);
        await _unitOfWork.CommitAsync();
    }
}