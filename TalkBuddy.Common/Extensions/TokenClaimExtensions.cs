using System.Security.Claims;

namespace TalkBuddy.Common.Extensions
{
    public static class TokenClaimExtensions
    {
        public static void TryGetValue(this IEnumerable<Claim> claims, string claimType, out string value)
        {
            value = claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;
        }

        public static bool IsValidClaim(this string claim)
        {
            return !string.IsNullOrEmpty(claim);
        }
    }
}
