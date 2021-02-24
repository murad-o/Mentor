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
            var userCreated = await _registerService.RegisterAsync(registerModel);

            if (userCreated.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
