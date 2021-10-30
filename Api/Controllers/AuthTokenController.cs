﻿using System.Threading.Tasks;
using Api.Controllers.Common;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthTokenController : BaseController
    {
        private readonly IJwtTokenService _jwtTokenService;

        public AuthTokenController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }


        /// <summary>
        /// Update refresh token
        /// </summary>
        /// <param name="jwtTokenModel"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRefreshToken(JwtTokenModel jwtTokenModel)
        {
            var jwtToken = await _jwtTokenService.UpdateJwtTokenAsync(jwtTokenModel);
            return Ok(jwtToken);
        }


        /// <summary>
        /// Revoke all user refresh tokens
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RevokeRefreshTokens()
        {
            await _jwtTokenService.RemoveRefreshTokensAsync();
            return NoContent();
        }
    }
}
