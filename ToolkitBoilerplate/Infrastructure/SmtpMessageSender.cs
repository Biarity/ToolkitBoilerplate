using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ToolkitBoilerplate.Infrastructure
{
    public class SmtpMessageSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpClient _client;

        public SmtpMessageSender(IConfiguration configuration)
        {
            _configuration = configuration;

            _client = new SmtpClient()
            {
                Host = _configuration["Smtp:Host"],
                EnableSsl = Boolean.Parse(_configuration["Smtp:Ssl"]),
                Port = Int32.Parse(_configuration["Smtp:Port"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                Timeout = 100000
            };
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var mailMessage = new MailMessage
            {
                From = new MailAddress(RandomizeEmail(_configuration["Smtp:From"])),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            })
            {
                mailMessage.To.Add(email);
                await _client.SendMailAsync(mailMessage);
            }
        }

        private string RandomizeEmail(string email)
        {
            if (Boolean.Parse(_configuration["Smtp:RandomizeFrom"]))
            {
                // Optimizing for speed. Does not need to be crypto-secure.
                var randomChars = Guid.NewGuid().ToString().Replace("-", string.Empty);
                return email.Replace("@", $"-{randomChars}@");
            }
            else
            {
                return email;
            }
        }
    }
}
