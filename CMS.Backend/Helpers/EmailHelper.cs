using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CMS.Backend.Helpers
{
    public static class EmailHelper
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("haocms.demo@gmail.com", "dummy_app_password_123"), // Mật khẩu ứng dụng giả lập
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("haocms.demo@gmail.com", "HaoCMS DigitalGuard"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch
            {
                // Bỏ qua lỗi gửi mail trong môi trường Demo nếu account không hợp lệ
            }
        }
    }
}
