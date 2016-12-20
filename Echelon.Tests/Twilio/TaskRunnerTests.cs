using Echelon.Core.Helpers;
using Echelon.TaskRunner;
using NUnit.Framework;

namespace Echelon.Tests.TaskRunner
{
    public class TaskRunnerTests
    {
        [Test]
        public void Method_Scenario_Result()
        {
            WindowsServiceHelper.Start<TaskRunnerServer>("TaskRunnerService-Test");
        }
    }
}