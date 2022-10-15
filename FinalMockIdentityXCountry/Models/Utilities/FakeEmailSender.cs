using Microsoft.AspNetCore.Identity.UI.Services;

namespace FinalMockIdentityXCountry.Models.Utilities
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
