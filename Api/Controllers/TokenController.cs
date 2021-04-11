using System.Threading.Tasks;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IExpiredTokenService _expiredTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ITokenGenerator _tokenGenerator;

        public TokenController(IExpiredTokenService expiredTokenService,
            IRefreshTokenService refreshTokenService, ITokenGenerator tokenGenerator)
        {
            _expiredTokenService = expiredTokenService;
            _refreshTokenService = refreshTokenService;
            _tokenGenerator = tokenGenerator;
        }


        [HttpPut]
        public async Task<IActionResult> RefreshToken(JwtTokenModel jwtTokenModel)
        {
            User user;
            try
            {
                user = await _expiredTokenService.GetUserFromExpiredTokenAsync(jwtTokenModel.AccessToken);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }

            var oldRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(jwtTokenModel.RefreshToken);

            if (oldRefreshToken is null || oldRefreshToken.UserId != user.Id)
                return BadRequest("Invalid client request");

            if (_refreshTokenService.IsTokenExpired(oldRefreshToken))
            {
                await _refreshTokenService.RemoveRefreshTokenAsync(oldRefreshToken);
                return Unauthorized();
            }

            await _refreshTokenService.RemoveRefreshTokenAsync(oldRefreshToken);

            var newAccessToken = _tokenGenerator.GenerateAccessToken(user);
            var newRefreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);

            return Ok(new { newAccessToken, newRefreshToken });
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshTokens()
        {
            var username = User.Identity!.Name;
            await _refreshTokenService.RemoveRefreshTokensAsync(username);

            return NoContent();
        }
    }
}
