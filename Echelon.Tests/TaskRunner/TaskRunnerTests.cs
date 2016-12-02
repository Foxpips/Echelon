using Echelon.TaskRunner;
using NUnit.Framework;

namespace Echelon.Tests.TaskRunner
{
    public class TaskRunnerTests
    {
        [Test]
        public void Method_Scenario_Result()
        {
            new TaskRunnerServer().Start();
        }
    }
}