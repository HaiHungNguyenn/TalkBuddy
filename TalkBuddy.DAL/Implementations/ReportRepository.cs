using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(TalkBuddyContext dbContext) : base(dbContext)
    {
    }
}