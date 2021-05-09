using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using MentorCore.Interfaces.Account;
using MentorCore.Interfaces.Jwt;
using Microsoft.AspNetCore.Http;

namespace MentorCore.Services.Account
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IAccessTokenService _accessTokenService;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IAccessTokenService accessTokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _accessTokenService = accessTokenService;
        }

        public async Task<User> GetCurrentUser()
        {
            var accessToken = GetJwtAccessToken();

            var user = await _accessTokenService.GetUserFromAccessTokenAsync(accessToken);
            return user;
        }

        private string GetJwtAccessToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
        }
    }
}
