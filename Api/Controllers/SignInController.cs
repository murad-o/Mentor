using System.Threading.Tasks;
using Abstractions.Account;
using Api.Controllers.Common;
using Contracts.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SignInController : BaseController
    {
        private readonly IAccountService _accountService;

        public SignInController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtTokenModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var jwtTokenModel = await _accountService.SignInAsync(loginModel);
            return Ok(jwtTokenModel);
        }
    }
}
