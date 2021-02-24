﻿using AutoMapper;
using Entities.Models;
using MentorCore.DTO.Account;

namespace MentorCore.Services.Automapper
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<RegisterModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}