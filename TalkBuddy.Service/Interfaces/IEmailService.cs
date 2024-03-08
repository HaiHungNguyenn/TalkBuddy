using TalkBuddy.Service.Models.Common;

namespace TalkBuddy.Service.Interfaces;

public interface IEmailService
{
    Task SendOptEmailAsync(OtpEmail email);
}