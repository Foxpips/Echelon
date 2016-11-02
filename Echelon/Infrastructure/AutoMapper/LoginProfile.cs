using AutoMapper;
using Echelon.Core.Helpers;
using Echelon.Entities.Users;
using Echelon.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace Echelon.Infrastructure.AutoMapper
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginViewModel, LoginEntity>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RememberMe, opt => opt.MapFrom(src => src.RememberMe))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => HashHelper.CreateHash(src.Password)));

            CreateMap<ExternalLoginInfo, LoginEntity>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.DefaultUserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RememberMe, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => string.Empty));
        }
    }
}