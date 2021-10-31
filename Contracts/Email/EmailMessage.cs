using MimeKit;

namespace Contracts.Email
{
    public class EmailMessage
    {
        public MailboxAddress To { get; init; }
        public string Subject { get; init; }
        public string Content { get; init; }

        public EmailMessage(string to, string subject, string content)
        {
            To = new MailboxAddress(to);
            Subject = subject;
            Content = content;
        }
    }
}
