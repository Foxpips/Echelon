using Echelon.Core.Infrastructure.Services.Windows;

namespace Echelon.TaskRunner
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            WindowsServiceHelper.Start<MassTransitService>("TaskRunner");
        }
    }
}