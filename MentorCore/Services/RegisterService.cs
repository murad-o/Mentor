using System.Threading.Tasks;
using AutoMapper;
using Entities.Data;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MentorCore.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegisterService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> Register(RegisterModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);
            var userCreated = await _userManager.CreateAsync(user, registerModel.Password);

            return userCreated;
        }
    }
}
