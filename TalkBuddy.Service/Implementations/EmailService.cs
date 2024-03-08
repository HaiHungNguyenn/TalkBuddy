using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using TalkBuddy.Service.Interfaces;
using TalkBuddy.Service.Models.Common;
using TalkBuddy.Service.Settings;

namespace TalkBuddy.Service.Implementations;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IConfiguration configuration)
    {
        _emailSettings = configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>() ??
                         throw new Exception("Missing email settings");
    }
    
    public async Task SendOptEmailAsync(OptEmail optEmail)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("TalkBuddy"));
        email.To.Add(MailboxAddress.Parse(optEmail.To));
        email.Subject = optEmail.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = optEmail.GetParsedBody() };
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.Host, 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}