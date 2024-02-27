using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Models.Requests;

namespace TalkBuddy.Service.Interfaces
{
	public interface IFacebookOAuthService
	{
		Task<Client?> ContinueWithFacebookAsync(FacebookOAuthRequest request);
	}
}
