using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.DAL.Implementations
{
	public class OtpCodeRepository : GenericRepository<OtpCode>, IOtpCodeRepository
	{
		public OtpCodeRepository(TalkBuddyContext dbContext) : base(dbContext)
		{
		}
	}
}
