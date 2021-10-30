using System.Threading.Tasks;
using Api.Controllers.Common;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Account;
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
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var jwtTokenModel = await _accountService.SignInAsync(loginModel);
            return Ok(jwtTokenModel);
        }
    }
}
