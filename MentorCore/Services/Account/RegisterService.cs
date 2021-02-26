using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Interfaces;
using MentorCore.Models.Email;
using Microsoft.AspNetCore.Identity;

namespace MentorCore.Services.Account
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public RegisterService(UserManager<User> userManager, IMapper mapper, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task RegisterAsync(RegisterModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);
            var userCreated = await _userManager.CreateAsync(user, registerModel.Password);

            if (userCreated.Succeeded)
            {
                var emailMessage = new EmailMessage(registerModel.Email, "Mentor", "Как ты вацок");
                await _emailSender.SendAsync(emailMessage);
            }
        }
    }
}
