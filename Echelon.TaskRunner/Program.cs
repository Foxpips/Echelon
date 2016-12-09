using Echelon.Core.Helpers;

namespace Echelon.TaskRunner
{
    internal class Program
    {
        private static void Main()
        {
            WindowsServiceHelper.Start<TaskRunnerServer>("TaskRunner");
        }
    }
}