using System;
using System.Linq;
using Autofac;
using FluentValidation;

namespace Echelon.Infrastructure.Autofac.Modules
{
    public class ValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            foreach (var validators in GetType().Assembly.ExportedTypes.Where(type => type.IsAssignableTo<IValidator>()))
            {
                var validatorInterface = validators.GetInterfaces().Single(IsValidator);
                builder.RegisterType(validators).As(validatorInterface).SingleInstance();
            }
        }

        private static bool IsValidator(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IValidator<>);
        }
    }
}