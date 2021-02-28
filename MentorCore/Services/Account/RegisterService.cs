using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Interfaces.Account;
using MentorCore.Interfaces.Email;
using MentorCore.Models.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace MentorCore.Services.Account
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public RegisterService(IMapper mapper, UserManager<User> userManager, IEmailSender emailSender,
            LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);
            var userRegistered = await _userManager.CreateAsync(user, registerModel.Password);

            if (userRegistered.Succeeded)
            {
                var emailConfirmationLink = await GenerateEmailConfirmationLink(user);

                var emailMessage = new EmailMessage(registerModel.Email, "Подтверждение почты",
                    $"Подтвердите регистрацию, перейдя по данной ссылке: {emailConfirmationLink}");

                await _emailSender.SendAsync(emailMessage);
            }

            return userRegistered;
        }

        private async Task<string> GenerateEmailConfirmationLink(User user)
        {
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Todo: Replace reference to 'Register' action method with 'ConfirmEmail' action method in email confirmation link
            var emailConfirmationLink = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, "Register", "Account",
                new {email = user.Email, token = emailConfirmationToken});

            return emailConfirmationLink;
        }
    }
}
