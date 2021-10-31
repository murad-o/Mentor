using System.Threading.Tasks;
using Abstractions.Account;
using Api.Controllers.Common;
using Contracts.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SignOutController : BaseController
    {
        private readonly IAccountService _accountService;

        public SignOutController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        /// <summary>
        /// Log out
        /// </summary>
        /// <param name="logoutModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Logout(LogoutModel logoutModel)
        {
            await _accountService.SignOutAsync(logoutModel);
            return NoContent();
        }
    }
}
