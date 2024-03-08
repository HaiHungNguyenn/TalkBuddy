namespace TalkBuddy.Common.Helpers;

public static class StringInterpolationHelper
{
    public static string TrimSpaceString(this string content)
    {
        return content.Replace(" ", "");
    }

    public static string GenerateUniqueFileName(string fileName,int length)
    {
        if (length <= 0) throw new Exception("Invalid Length");
        var originalName = fileName.Split(".")[0];
        var extenstionName = fileName.Split(".")[1];
        var uniqueFileName = originalName + RandomPasswordHelper.GenerateRandomPassword(length) + "." + extenstionName;
        return uniqueFileName.TrimSpaceString();
    }
}