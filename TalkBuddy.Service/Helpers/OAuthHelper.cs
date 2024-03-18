using TalkBuddy.Service.Settings;

namespace TalkBuddy.Common.Helpers;
public static class OAuthHelper
{
    public static string BuildGoogleOauthUrl(GoogleSettings googleSettings)
    {
        return $"https://accounts.google.com/o/oauth2/v2/auth?client_id={googleSettings.ClientId}&scope=email%20profile%20openid&redirect_uri={googleSettings.RedirectUri}&response_type=code";
    }

    public static string BuildFacebookOauthUrl(FacebookSettings facebookSettings)
    {
        return $"https://www.facebook.com/v19.0/dialog/oauth?client_id={facebookSettings.ClientId}&scope=email%2Cpublic_profile&response_type=token&redirect_uri={facebookSettings.RedirectUri}";
    }
}
