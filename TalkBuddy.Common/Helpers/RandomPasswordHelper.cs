namespace TalkBuddy.Common.Helpers
{
    public static class RandomPasswordHelper
    {
        public static string GenerateRandomPassword(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const string sepecialChars = "!@#$%^&*()_+";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
            .ToArray()
                .Append(sepecialChars[random.Next(sepecialChars.Length)]).ToArray());
        }
    }
}
