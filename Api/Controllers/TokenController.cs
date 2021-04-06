using System.Threading.Tasks;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJsonExpiredTokenService _jsonExpiredTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJsonTokenGenerator _jsonTokenGenerator;

        public TokenController(IJsonExpiredTokenService jsonExpiredTokenService, UserManager<User> userManager,
            IRefreshTokenService refreshTokenService, IJsonTokenGenerator jsonTokenGenerator)
        {
            _jsonExpiredTokenService = jsonExpiredTokenService;
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _jsonTokenGenerator = jsonTokenGenerator;
        }


        [HttpPut]
        public async Task<IActionResult> RefreshToken(JwtTokenModel jwtTokenModel)
        {
            var principal = _jsonExpiredTokenService.GetPrincipalFromExpiredToken(jwtTokenModel.AccessToken);

            var username = principal.Identity?.Name;
            var user = await _userManager.FindByEmailAsync(username);

            var oldRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(jwtTokenModel.RefreshToken);

            if (user is null || oldRefreshToken is null || oldRefreshToken.Token != jwtTokenModel.RefreshToken)
                return BadRequest("Invalid client request");

            if (_refreshTokenService.IsTokenExpired(oldRefreshToken))
                return Unauthorized();

            await _refreshTokenService.SetRefreshTokenStatusToUsedAsync(oldRefreshToken);

            var newAccessToken = _jsonTokenGenerator.GenerateAccessToken(principal.Claims);
            var newRefreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);

            return Ok(new { newAccessToken, newRefreshToken });
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshTokens()
        {
            var username = User.Identity!.Name;

            await _refreshTokenService.RevokeRefreshTokensAsync(username);

            return NoContent();
        }
    }
}
