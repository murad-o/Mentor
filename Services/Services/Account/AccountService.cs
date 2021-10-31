using System.Linq;
using System.Threading.Tasks;
using Abstractions.Account;
using Abstractions.Email;
using Abstractions.Jwt;
using AutoMapper;
using Contracts.Account;
using Contracts.Email;
using Contracts.Users;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace Services.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly IEmailSender _emailSender;

        public AccountService(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager,
            IJwtTokenGenerator jwtTokenGenerator, IJwtTokenService jwtTokenService,
            IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtTokenService = jwtTokenService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _emailSender = emailSender;
        }

        public async Task SignUpAsync(RegisterModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);
            var userCreated = await _userManager.CreateAsync(user, registerModel.Password);

            if (!userCreated.Succeeded)
            {
                var errorResult = string.Empty;
                errorResult = userCreated.Errors.Aggregate(errorResult, (current, error) => current + error.Description);
                throw new BadRequestException(errorResult);
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailConfirmationLink = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext,
                "ConfirmEmail", "Account", new {email = user.Email, token = emailConfirmationToken});

            var emailMessage = new EmailMessage(user.Email, "Confirming email",
                $"To confirm email follow the link: {emailConfirmationLink}");
            await _emailSender.SendAsync(emailMessage);
        }

        public async Task<JwtTokenModel> SignInAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user is null)
                throw new BadRequestException("Incorrect username or password");

            var authorized = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!user.EmailConfirmed)
                throw new BadRequestException("Confirm your email");

            if (!authorized.Succeeded)
                throw new BadRequestException("Incorrect username or password");

            var userModel = _mapper.Map<UserModel>(user);

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(userModel);
            var refreshToken = await _jwtTokenService.CreateRefreshTokenAsync(userModel);

            return new JwtTokenModel {AccessToken = accessToken, RefreshToken = refreshToken};
        }

        public async Task SignOutAsync(LogoutModel logoutModel)
        {
            await _jwtTokenService.RemoveRefreshTokenAsync(logoutModel.RefreshToken);
        }

        public async Task ConfirmEmailAsync(EmailConfirmationModel emailModel)
        {
            var user = await _userManager.FindByEmailAsync(emailModel.Email);

            if (user is null)
                throw new NotFoundException($"User with the email {emailModel.Email} not found");

            var emailConfirmed = await _userManager.ConfirmEmailAsync(user, emailModel.Token);

            if (!emailConfirmed.Succeeded)
                throw new BadRequestException("Invalid token");
        }
    }
}
