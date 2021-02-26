using System.Threading.Tasks;
using MentorCore.Models.Email;

namespace MentorCore.Interfaces
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage emailMessage);
    }
}
