using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Extensions;
using MentorCore.Interfaces.Email;
using MentorCore.Models.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AccountController(IMapper mapper, UserManager<User> userManager,
            IEmailSender emailSender, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);
            var userCreated = await _userManager.CreateAsync(user, registerModel.Password);

            if (!userCreated.Succeeded)
            {
                AddModelErrors(userCreated);
                return BadRequest(ModelState);
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailConfirmationLink = Url.Action("ConfirmEmail", "Account",
                new {email = user.Email, token = emailConfirmationToken}, Request.Scheme);

            var emailMessage = new EmailMessage(user.Email, "Подтверждение почты",
                $"Подтвердите регистрацию, перейдя по данной ссылке: {emailConfirmationLink}");
            await _emailSender.SendAsync(emailMessage);

            return Ok();
        }

        [HttpGet]
        [Route("email/confirmation")]
        public async Task<ActionResult> ConfirmEmail([FromQuery] EmailConfirmationModel emailModel)
        {
            var user = await _userManager.FindByEmailAsync(emailModel.Email);

            if (user is null)
                return NotFound("User is not found");

            var emailConfirmed = await _userManager.ConfirmEmailAsync(user, emailModel.Token);

            if (emailConfirmed.Succeeded)
                return Content("Your email confirmed successfully");

            AddModelErrors(emailConfirmed);
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = _mapper.Map<User>(loginModel);
            var authorized = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!authorized.Succeeded)
                return Unauthorized();

            var jwtConfigurations = _configuration.GetJwtConfigurations();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigurations.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtConfigurations.ValidIssuer,
                audience: jwtConfigurations.ValidAudience,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(jwtConfigurations.LifeTime),
                signingCredentials: signinCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new {token});
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
