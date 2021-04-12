using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Email;
using MentorCore.Models.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SignUpController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public SignUpController(IMapper mapper, UserManager<User> userManager,
            IEmailSender emailSender)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        [HttpPost]
        public async Task<ActionResult> SignUp(RegisterModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);
            var userCreated = await _userManager.CreateAsync(user, registerModel.Password);

            if (!userCreated.Succeeded)
            {
                foreach (var error in userCreated.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailConfirmationLink = Url.Action("ConfirmEmail", "Account",
                new {email = user.Email, token = emailConfirmationToken}, Request.Scheme);

            var emailMessage = new EmailMessage(user.Email, "Confirming email",
                $"To confirm email follow the link: {emailConfirmationLink}");
            await _emailSender.SendAsync(emailMessage);

            return Ok();
        }
    }
}
