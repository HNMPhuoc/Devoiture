using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace Devoiture.Helpers
{
    public class EmailSender
    {
        private readonly SMTPConfig _smtpConfig;

        public EmailSender(IOptions<SMTPConfig> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public void SendMail(string to, string subject, string content)
        {
            var fromAddress = new MailAddress(_smtpConfig.From, _smtpConfig.DisplayName);
            var toAddress = new MailAddress(to);
            var smtp = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _smtpConfig.Password)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = content,
                IsBodyHtml = _smtpConfig.IsHtml
            })
            {
                smtp.Send(message);
            }
        }
    }
}