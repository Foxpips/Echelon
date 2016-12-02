using Autofac;
using Echelon.Infrastructure.Services.Category;
using Echelon.Infrastructure.Services.Login;

namespace Echelon.Infrastructure.AutoFac.Modules
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