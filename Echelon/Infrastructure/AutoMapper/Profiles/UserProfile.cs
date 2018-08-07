using System;
using AutoMapper;
using Echelon.Data.Entities.Users;
using Echelon.Mediators;
using Echelon.Models.ViewModels;

namespace Echelon.Infrastructure.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<TempUserEntity, UserEntity>()
                .ForMember(dest => dest.UniqueIdentifier, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RememberMe, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src =>src.Password));

            CreateMap<UserEntity, ProfileViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName));

            CreateMap<UserEntity, ResetPasswordViewModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
        }
    }
}