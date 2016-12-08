using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;

namespace Echelon.Core.Extensions.AutoMapper
{
    public static class AutoMapperExtensions
    {
        public static void RegisterProfilesOnInit(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var profiles = assembly.ExportedTypes.Where(type => type.IsAssignableTo<Profile>());

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
}