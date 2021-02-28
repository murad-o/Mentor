using System.Threading.Tasks;
using MentorCore.DTO.Account;
using MentorCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public AccountController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            var userRegistered = await _registerService.RegisterAsync(registerModel);

            if (userRegistered.Succeeded)
            {
                return Ok();
            }

            foreach (var error in userRegistered.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
    }
}
