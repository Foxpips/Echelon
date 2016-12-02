using Topshelf;

namespace Echelon.TaskRunner
{
    public class TaskRunnerWindowsService
    {
        public void Initialize()
        {
            const string serviceName = "TaskRunnerService";

            HostFactory.Run(
                x =>
                {
                    x.Service<TaskRunnerServer>(
                        s =>
                        {
                            s.ConstructUsing(f => new TaskRunnerServer());
                            s.WhenStarted(tc => tc.Start());
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