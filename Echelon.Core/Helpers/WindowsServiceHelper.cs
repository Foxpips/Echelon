using System;
using Echelon.Core.Interfaces;
using Topshelf;

namespace Echelon.Core.Helpers
{
    public class WindowsServiceHelper
    {
        public static void Start<TService>(string serviceName, Action<TService> action = null) where TService : class, IService, new()
        {
            HostFactory.Run(
                x =>
                {
                    x.Service<TService>(
                        s =>
                        {
                            s.ConstructUsing(f => new TService());
                            s.WhenStarted(tc =>
                            {
                                tc.Initialize();
                                action?.Invoke(tc);
                            });
                            s.WhenStopped(tc => { });
                        });

                    x.RunAsLocalSystem();
                    x.SetDescription(serviceName);
                    x.SetDisplayName(serviceName);
                    x.SetServiceName(serviceName);
                    x.StartAutomaticallyDelayed();

                    x.EnableServiceRecovery(r =>
                    {
                        r.RestartService(1);
                        r.RestartService(2);
                    });
                });
        }
    }
}