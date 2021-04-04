using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Email;
using MentorCore.Interfaces.Jwt;
using MentorCore.Models.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IJsonTokenGenerator _jsonTokenGenerator;
        private readonly IJsonExpiredTokenService _jsonExpiredTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AccountController(IMapper mapper, UserManager<User> userManager,
            IEmailSender emailSender, SignInManager<User> signInManager,
            IJsonTokenGenerator jsonTokenGenerator, IJsonExpiredTokenService jsonExpiredTokenService,
            IRefreshTokenService refreshTokenService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _jsonTokenGenerator = jsonTokenGenerator;
            _jsonExpiredTokenService = jsonExpiredTokenService;
            _refreshTokenService = refreshTokenService;
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
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user is null)
            {
                ModelState.TryAddModelError("IncorrectCredentials", "Incorrect UserName or Password");
                return BadRequest(ModelState);
            }

            var authorized = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!authorized.Succeeded)
            {
                ModelState.TryAddModelError("IncorrectCredentials", "Incorrect UserName or Password");
                return BadRequest(ModelState);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, loginModel.Email)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var accessToken = _jsonTokenGenerator.GenerateAccessToken(claims);
            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);

            return Ok(new {accessToken, refreshToken});
        }

        [HttpPut]
        [Route("token")]
        public async Task<IActionResult> RefreshToken(JwtTokenModel jwtTokenModel)
        {
            var principal = _jsonExpiredTokenService.GetPrincipalFromExpiredToken(jwtTokenModel.AccessToken);

            var username = principal.Identity?.Name;
            var user = await _userManager.FindByEmailAsync(username);

            var oldRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(jwtTokenModel.RefreshToken);

            if (user is null || oldRefreshToken is null || oldRefreshToken.Token != jwtTokenModel.RefreshToken)
                return BadRequest("Invalid client request");

            if (_refreshTokenService.IsTokenExpired(oldRefreshToken))
                return Unauthorized();

            await _refreshTokenService.SetRefreshTokenStatusToUsedAsync(oldRefreshToken);

            var newAccessToken = _jsonTokenGenerator.GenerateAccessToken(principal.Claims);
            var newRefreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }

        [HttpDelete]
        [Authorize]
        [Route("token")]
        public async Task<IActionResult> RevokeRefreshToken()
        {
            var username = User.Identity?.Name;

            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                return BadRequest();

            await _refreshTokenService.RevokeRefreshTokenAsync(user);

            return NoContent();
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
