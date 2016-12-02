using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Echelon.Core.Extensions.Autofac;

namespace Echelon.Core.Extensions.AutoMapper
{
    public static class AutoMapperExtensions
    {
        public static void RegisterProfilesOnInit()
        {
            var profiles = typeof(AutofacExtensions).GetTypeInfo()
                .Assembly.ExportedTypes.Where(type => type.IsAssignableTo<Profile>());

            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }
    }
}