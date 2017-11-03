﻿using System;
using AutoMapper;
using Echelon.Core.Helpers;
using Echelon.Data.Entities.Users;
using Echelon.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace Echelon.Infrastructure.AutoMapper.Profiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginViewModel, UserEntity>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RememberMe, opt => opt.MapFrom(src => src.RememberMe))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => HashHelper.CreateHash(src.Password)));

            CreateMap<ExternalLoginInfo, UserEntity>()
                .ForMember(dest => dest.UniqueIdentifier, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DefaultUserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RememberMe, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => string.Empty));
        }
    }
}