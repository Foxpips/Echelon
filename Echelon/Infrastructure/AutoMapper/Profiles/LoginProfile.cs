using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Entities.Users;
using Echelon.Core.Extensions;
using Echelon.Core.Infrastructure.Services.Rest;
using Echelon.Infrastructure.Settings;
using Echelon.Models.BusinessModels;
using Echelon.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace Echelon.Infrastructure.AutoMapper.Profiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile(IRestService restService)
        {
            CreateMap<LoginViewModel, UserEntity>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RememberMe, opt => opt.MapFrom(src => src.RememberMe))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => HashHelper.CreateHash(src.Password)));

            CreateMap<ExternalLoginInfo, UserEntity>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.DefaultUserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
//TODO fix broken async                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => SetGoogleAvatar(src, restService)))
                .ForMember(dest => dest.RememberMe, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => string.Empty));
        }

        private static async Task<string> SetGoogleAvatar(ExternalLoginInfo externalLoginInfoAsync, IRestService restService)
        {
            var requestUri =
                new Uri(SiteSettings.GoogleProfileUri + externalLoginInfoAsync.ExternalIdentity.Claims.Where(
                    c => c.Type.Equals(SiteSettings.GoogleAccessToken))
                    .Select(c => c.Value)
                    .FirstOrDefault());

            return (await restService.MakeGenericRequest<GooglePlusInfo>(requestUri)).Picture;
        }
    }
}