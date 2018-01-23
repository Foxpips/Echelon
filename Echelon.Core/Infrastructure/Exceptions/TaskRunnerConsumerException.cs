using System;

namespace Echelon.Core.Infrastructure.Exceptions
{
    public class TaskRunnerConsumerException : Exception
    {
        public TaskRunnerConsumerException(string message) : base(message)
        {
        }
    }
}