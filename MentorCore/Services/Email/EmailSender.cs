using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MentorCore.Interfaces.Email;
using MentorCore.Models.Email;
using MimeKit;

namespace MentorCore.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly SmtpConfiguration _smtpConfig;

        public EmailSender(EmailConfiguration emailConfig, SmtpConfiguration smtpConfig)
        {
            _emailConfig = emailConfig;
            _smtpConfig = smtpConfig;
        }

        public async Task SendAsync(EmailMessage emailMessage)
        {
            var mimeMessage = CreateMessage(emailMessage);

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpConfig.Server, _smtpConfig.Port, true);

            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

            await client.SendAsync(mimeMessage);

            await client.DisconnectAsync(true);
        }

        private MimeMessage CreateMessage(EmailMessage emailMessage)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_emailConfig.UserName));
            mimeMessage.To.Add(emailMessage.To);
            mimeMessage.Subject = emailMessage.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = emailMessage.Content
            };

            return mimeMessage;
        } 
    }
}
