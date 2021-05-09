using System.Threading.Tasks;
using Entities.Models;

namespace MentorCore.Interfaces.Account
{
    public interface ICurrentUserService
    {
        Task<User> GetCurrentUser();
    }
}
