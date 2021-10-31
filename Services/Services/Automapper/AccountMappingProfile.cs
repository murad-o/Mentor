using AutoMapper;
using Contracts.Account;
using Contracts.Users;
using Domain.Entities;

namespace Services.Services.Automapper
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<RegisterModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<User, UserModel>();
        }
    }
}
