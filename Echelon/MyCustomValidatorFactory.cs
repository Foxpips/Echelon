using System;
using Autofac;
using FluentValidation;

namespace Echelon
{
    public class MyCustomValidatorFactory : ValidatorFactoryBase
    {
        private readonly IContainer _container;

        public MyCustomValidatorFactory(IContainer container)
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