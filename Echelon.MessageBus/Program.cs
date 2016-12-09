using System;
using Echelon.Core.Helpers;
using Echelon.Objects.Infrastructure.MassTransit.Commands;

namespace Echelon.MessageBus
{
    internal class Program
    {
        private static void Main()
        {
            WindowsServiceHelper.Start<TransitBusServer>("TransitBusServer", server =>
            {
                var bus = server.Bus;
                bus.Publish(new LogInfoCommand { Content = new Random(100).Next().ToString() });
            });
        }
    }
}