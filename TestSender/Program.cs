using Messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSender
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("Test.Endpoint");

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .ConnectionString("Connection string");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendOnly();

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            await endpointInstance.Send("Test.Endpoint", new TestCommand { Id = 42 });
        }

    }
}
