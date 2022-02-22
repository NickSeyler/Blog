using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace Blog.Services
{
    public class BasicEmailService : IEmailSender
    {
        private readonly IConfiguration _appSettings;

        public BasicEmailService(IConfiguration appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MimeMessage newEmail = new();
            var userName = _appSettings["SmtpSettings:UserName"];


            newEmail.Sender = MailboxAddress.Parse(userName);
            newEmail.To.Add(MailboxAddress.Parse(email));
            newEmail.Subject = subject;

            BodyBuilder emailBody = new();
            emailBody.HtmlBody = htmlMessage;
            newEmail.Body = emailBody.ToMessageBody();

            using SmtpClient smtpClient = new();

            var host = _appSettings["SmtpSettings:Host"];
            var port = Convert.ToInt32(_appSettings["SmtpSettings:Port"]);

            await smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(userName, _appSettings["SmtpSettings:Password"]);

            await smtpClient.SendAsync(newEmail);
            await smtpClient.DisconnectAsync(true);
        }
    }
}
