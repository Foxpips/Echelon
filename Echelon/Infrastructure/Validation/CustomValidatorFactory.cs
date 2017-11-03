using System;
using Autofac;
using FluentValidation;

namespace Echelon.Infrastructure.Validation
{
    public class CustomValidatorFactory : ValidatorFactoryBase
    {
        private readonly IContainer _container;

        public CustomValidatorFactory(IContainer container)
        {
            _container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            object resolveable;
            var tryResolve = _container.TryResolve(validatorType, out resolveable);
            if (tryResolve)
            {
                return resolveable as IValidator;
            }

            return null;
        }
    }
}