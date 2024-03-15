using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces;

public interface IReportService
{
    Task CreateReport(Report report);
}