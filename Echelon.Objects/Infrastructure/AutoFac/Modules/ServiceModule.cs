using Autofac;
using Echelon.Core.Infrastructure.Services.Category;
using Echelon.Core.Infrastructure.Services.Login;

namespace Echelon.Core.Infrastructure.AutoFac.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
            builder.RegisterType<CategoryService>().As<ICategoryService>().SingleInstance();
        }
    }
}