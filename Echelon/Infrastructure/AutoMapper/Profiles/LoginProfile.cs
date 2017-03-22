using AutoMapper;
using Echelon.Core.Extensions;
using Echelon.Core.Helpers;
using Echelon.Core.Infrastructure.Services.Rest;
using Echelon.Data.Entities.Users;
using Echelon.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace Echelon.Infrastructure.AutoMapper.Profiles
{
    public class LoginProfile : Profile
    {
        private IRestService _restService;

        public LoginProfile(IRestService restService)
        {
            _restService = restService;
            CreateMap<LoginViewModel, UserEntity>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RememberMe, opt => opt.MapFrom(src => src.RememberMe))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => HashHelper.CreateHash(src.Password)));

            CreateMap<ExternalLoginInfo, UserEntity>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.DefaultUserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RememberMe, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => string.Empty));
        }
    }
}