using System.Threading.Tasks;
using Api.Controllers.Common;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SignOutController : BaseController
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public SignOutController(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }


        /// <summary>
        /// Log out
        /// </summary>
        /// <param name="logoutModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout(LogoutModel logoutModel)
        {
            var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(logoutModel.RefreshToken);

            if (refreshToken is null)
                return NotFound("Refresh token is not found");

            await _refreshTokenService.RemoveRefreshTokenAsync(refreshToken);
            return NoContent();
        }
    }
}
