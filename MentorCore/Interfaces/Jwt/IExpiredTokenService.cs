using System.Threading.Tasks;
using Entities.Models;

namespace MentorCore.Interfaces.Jwt
{
    public interface IExpiredTokenService
    {
        Task<User> GetUserFromExpiredTokenAsync(string expiredToken);
    }
}
