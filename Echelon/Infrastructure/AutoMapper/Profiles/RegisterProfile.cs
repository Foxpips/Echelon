using AutoMapper;
using Echelon.Core.Helpers;
using Echelon.Data.Entities.Users;
using Echelon.Models.ViewModels;

namespace Echelon.Infrastructure.AutoMapper.Profiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterViewModel, TempUserEntity>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => HashHelper.CreateHash(src.Password)));
        }
    }
}