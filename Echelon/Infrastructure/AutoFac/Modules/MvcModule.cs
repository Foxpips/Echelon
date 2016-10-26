using Autofac;
using Autofac.Integration.Mvc;

namespace Echelon.Infrastructure.AutoFac.Modules
{
    public class MvcModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
        }
    }
}