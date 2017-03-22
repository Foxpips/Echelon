using Echelon.Core.Infrastructure.Services.Windows;

namespace TaskRunner
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            WindowsServiceHelper.Start<MassTransitService>("TaskRunner");
        }
    }
}