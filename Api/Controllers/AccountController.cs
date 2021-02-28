using System.Threading.Tasks;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Account;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        private readonly IEmailConfirmationService _emailConfirmationService;

        public AccountController(IRegisterService registerService, IEmailConfirmationService emailConfirmationService)
        {
            _registerService = registerService;
            _emailConfirmationService = emailConfirmationService;
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

        [HttpGet]
        [Route("email/confirmation")]
        public async Task<ActionResult> ConfirmEmail(string email, string token)
        {
            var emailConfirmed = await _emailConfirmationService.ConfirmEmailAsync(email, token);

            if (emailConfirmed.Succeeded)
                return Ok();

            foreach (var error in emailConfirmed.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
    }
}
