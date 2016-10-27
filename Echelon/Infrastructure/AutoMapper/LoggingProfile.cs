using AutoMapper;
using Echelon.Entities;
using Echelon.Entities.Users;
using Echelon.Models.ViewModels;

namespace Echelon.Infrastructure.AutoMapper
{
    public class LoggingProfile : Profile
    {
        public LoggingProfile()
        {
            CreateMap<LoginViewModel, LoginEntity>();
        }
    }
}