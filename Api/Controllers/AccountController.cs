using System.Threading.Tasks;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Account;
using Microsoft.AspNetCore.Identity;
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
                return Ok();

            AddModelErrors(userRegistered);

            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("email/confirmation")]
        public async Task<ActionResult> ConfirmEmail(string email, string token)
        {
            var emailConfirmed = await _emailConfirmationService.ConfirmEmailAsync(email, token);

            if (emailConfirmed.Succeeded)
                return Ok();

            AddModelErrors(emailConfirmed);

            return BadRequest(ModelState);
        }

        private void AddModelErrors(IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
        }
    }
}
