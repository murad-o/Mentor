using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Data;
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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IJwtExpiredTokenService _jwtExpiredTokenService;
        private readonly AppDbContext _context;

        public AccountController(IMapper mapper, UserManager<User> userManager,
            IEmailSender emailSender, SignInManager<User> signInManager,
            IJwtTokenGenerator jwtTokenGenerator, AppDbContext context, IJwtExpiredTokenService jwtExpiredTokenService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _context = context;
            _jwtExpiredTokenService = jwtExpiredTokenService;
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

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(claims);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();

            return Ok(new {accessToken, refreshToken});
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshJwtToken(JwtTokenModel jwtTokenModel)
        {
            var principal = _jwtExpiredTokenService.GetPrincipalFromExpiredToken(jwtTokenModel.AccessToken);

            var username = principal.Identity?.Name;
            var user = await _userManager.FindByEmailAsync(username);

            if (user is null || user.RefreshToken != jwtTokenModel.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }

        [HttpPost]
        [Authorize]
        [Route("revoke")]
        public async Task<IActionResult> RevokeJwtToken()
        {
            var username = User.Identity?.Name;

            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                return BadRequest();

            user.RefreshToken = null;
            await _context.SaveChangesAsync();

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
