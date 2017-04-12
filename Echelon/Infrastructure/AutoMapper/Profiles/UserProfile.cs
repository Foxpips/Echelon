using AutoMapper;
using Echelon.Data.Entities.Users;
using Echelon.Models.ViewModels;

namespace Echelon.Infrastructure.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, ProfileViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserNameEnabled, opt => opt.MapFrom(src => src.UserNameEnabled));

        }
    }
}