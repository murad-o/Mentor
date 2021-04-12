using System.Threading.Tasks;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SignOutController : ControllerBase
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public SignOutController(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }


        [HttpPost]
        public async Task<IActionResult> Logout(LogoutModel logoutModel)
        {
            var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(logoutModel.RefreshToken);

            if (refreshToken is not null)
                await _refreshTokenService.RemoveRefreshTokenAsync(refreshToken);

            return NoContent();
        }
    }
}
