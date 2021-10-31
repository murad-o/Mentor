using System.Threading.Tasks;
using Contracts.Email;

namespace Abstractions.Email
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage emailMessage);
    }
}
