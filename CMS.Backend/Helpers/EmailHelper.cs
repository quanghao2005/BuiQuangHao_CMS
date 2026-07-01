using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

using System.Collections.Generic;

namespace CMS.Backend.Helpers
{
    public static class EmailHelper
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body, Dictionary<string, string> inlineImages = null)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            string senderEmail = configuration["EmailSettings:Email"] ?? "haocms.demo@gmail.com";
            string senderPassword = configuration["EmailSettings:Password"] ?? "dummy";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "HaoCMS Store"),
                Subject = subject,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            var htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            if (inlineImages != null)
            {
                foreach (var img in inlineImages)
                {
                    if (File.Exists(img.Value))
                    {
                        var res = new LinkedResource(img.Value) { ContentId = img.Key };
                        htmlView.LinkedResources.Add(res);
                    }
                }
            }
            mailMessage.AlternateViews.Add(htmlView);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine($"[EmailHelper] Đã gửi email thành công đến {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailHelper] Lỗi gửi email: {ex.Message}");
                // Bỏ qua lỗi gửi mail trong môi trường Demo
            }
        }
    }
}
