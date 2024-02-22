using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Models.Requests;

namespace TalkBuddy.Service.Interfaces
{
    public interface IGoogleOAuthService
    {
        Task<Client?> ContinueWithGoogleAsync(GoogleOAuthRequest request);
    }
}
