using System.Threading.Tasks;
using Api.Controllers.Common;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    public class AuthTokenController : BaseController
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthTokenController(IAccessTokenService accessTokenService,
            IRefreshTokenService refreshTokenService, ITokenGenerator tokenGenerator)
        {
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _tokenGenerator = tokenGenerator;
        }


        [HttpPut]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel refreshTokenModel)
        {
            User user;
            try
            {
                user = await _accessTokenService.GetUserFromAccessTokenAsync(refreshTokenModel.AccessToken);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }

            var oldRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(refreshTokenModel.RefreshToken);

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
