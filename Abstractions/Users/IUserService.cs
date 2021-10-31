using System.Threading.Tasks;
using Contracts.Users;

namespace Abstractions.Users
{
    public interface IUserService
    {
        Task<UserModel> GetCurrentUser();
    }
}
