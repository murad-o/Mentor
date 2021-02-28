using System.Threading.Tasks;
using MentorCore.Models.Email;

namespace MentorCore.Interfaces.Email
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage emailMessage);
    }
}
