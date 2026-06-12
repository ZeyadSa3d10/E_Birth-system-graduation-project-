using E_Birth.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace E_Birth.Application.NotificationService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration config)
        {
            _configuration = config;
        }
        public async Task SendAsync(string to, string subject, string body)
        {
            var from = _configuration["EmailSettings:From"];
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);

            using var smtpClient = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Timeout = 10000
            };

            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
