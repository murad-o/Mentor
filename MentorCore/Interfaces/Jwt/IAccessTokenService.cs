using System.Threading.Tasks;
using Entities.Models;

namespace MentorCore.Interfaces.Jwt
{
    public interface IAccessTokenService
    {
        Task<User> GetUserFromAccessTokenAsync(string accessToken);
    }
}
