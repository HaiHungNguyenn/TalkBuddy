using DevOne.Security.Cryptography.BCrypt;

namespace TalkBuddy.Common.Helpers
{
	public static class PasswordHelper
	{
		public static string HashPassword(string password)
		{
			return BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt());
		}

		public static bool IsValidPassword(string rawPassword, string hashedPassword)
		{
			try
			{
				return BCryptHelper.CheckPassword(rawPassword, hashedPassword);
			} 
			catch
			{
				return false;
			}
		}
	}
}
