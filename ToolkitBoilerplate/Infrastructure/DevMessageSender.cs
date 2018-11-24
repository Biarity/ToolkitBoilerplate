using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

namespace ToolkitBoilerplate.Infrastructure
{
    public class DevEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            Console.WriteLine($"\nEMAIL SENT: \n" +
                $"TO: {email} \n" +
                $"SUBJECT: {subject} \n" +
                $"BODY: {message} \n");

            return Task.FromResult(0);
        }
    }
}
