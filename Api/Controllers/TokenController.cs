using System.Security.Claims;
using System.Threading.Tasks;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJsonExpiredTokenService _jsonExpiredTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ITokenGenerator _tokenGenerator;

        public TokenController(IJsonExpiredTokenService jsonExpiredTokenService, UserManager<User> userManager,
            IRefreshTokenService refreshTokenService, ITokenGenerator tokenGenerator)
        {
            _jsonExpiredTokenService = jsonExpiredTokenService;
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _tokenGenerator = tokenGenerator;
        }


        [HttpPut]
        public async Task<IActionResult> RefreshToken(JwtTokenModel jwtTokenModel)
        {
            ClaimsPrincipal claimsPrincipal;
            try
            {
                claimsPrincipal = _jsonExpiredTokenService.GetPrincipalFromExpiredToken(jwtTokenModel.AccessToken);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }

            var username = claimsPrincipal.Identity?.Name;
            var user = await _userManager.FindByEmailAsync(username);

            var oldRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(jwtTokenModel.RefreshToken);

            if (oldRefreshToken is null || oldRefreshToken.UserId != user.Id)
                return BadRequest("Invalid client request");

            if (_refreshTokenService.IsTokenExpired(oldRefreshToken))
                return Unauthorized();

            await _refreshTokenService.SetRefreshTokenStatusToUsedAsync(oldRefreshToken);

            var newAccessToken = _tokenGenerator.GenerateAccessToken(claimsPrincipal.Claims);
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
