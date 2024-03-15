using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TalkBuddy.Common.Constants;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.SignalR;

public class MediaHub : Hub
{
    private readonly IReportService _reportService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MediaHub(IReportService reportService, IHttpContextAccessor httpContextAccessor)
    {
        _reportService = reportService;
        _httpContextAccessor = httpContextAccessor;
    }
    public void SendReport(string userId, string detail, string url)
    {
        var report = new Report()
        {
            Details = detail,
            CreatedDate = DateTime.Now,
            ReportedClientId = new Guid(userId),
            InformantClientId = new Guid(_httpContextAccessor.HttpContext.Session.GetString(SessionConstants.USER_ID)),
        };
        report.ReportEvidences.Add(new ReportEvidence(){EvidenceUrl = url});
        _reportService.CreateReport(report);
    }
}