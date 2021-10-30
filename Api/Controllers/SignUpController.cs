using System.Threading.Tasks;
using Api.Controllers.Common;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Account;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SignUpController : BaseController
    {
        private readonly IAccountService _accountService;

        public SignUpController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        /// <summary>
        /// Sign up
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SignUp(RegisterModel registerModel)
        {
            await _accountService.SignUpAsync(registerModel);
            return Ok();
        }
    }
}
