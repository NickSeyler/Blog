using Microsoft.AspNetCore.Identity.UI.Services;

namespace Blog.Services
{
    public class BasicEmailService : IEmailSender
    {
        private readonly IConfiguration _appSettings;

        public BasicEmailService(IConfiguration appSettings)
        {
            _appSettings = appSettings;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
