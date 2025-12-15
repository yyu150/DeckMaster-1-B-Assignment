using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DeckMaster.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SendGridEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var fromEmail = _config["SendGrid:FromEmail"];
            var fromName = _config["SendGrid:FromName"] ?? "DeckMaster";

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(fromEmail))
                throw new InvalidOperationException("SendGrid secrets missing. Check User Secrets: SendGrid:ApiKey, SendGrid:FromEmail, SendGrid:FromName.");

            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                HtmlContent = htmlMessage
            };

            msg.AddTo(new EmailAddress(email));
            
            var response = await client.SendEmailAsync(msg);
            if ((int)response.StatusCode >= 400)
            {
                var body = await response.Body.ReadAsStringAsync();
                throw new InvalidOperationException($"SendGrid failed: {(int)response.StatusCode} {response.StatusCode}. {body}");
            }
        }
    }
}