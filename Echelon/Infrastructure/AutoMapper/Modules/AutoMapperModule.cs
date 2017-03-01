﻿using System.Collections.Generic;
using Autofac;
using AutoMapper;

namespace Echelon.Infrastructure.AutoMapper.Modules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //register all profile classes in the calling assembly
            builder.RegisterAssemblyTypes(GetType().Assembly).As<Profile>();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}