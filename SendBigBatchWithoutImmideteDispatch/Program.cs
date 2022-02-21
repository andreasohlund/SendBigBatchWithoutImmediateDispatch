using Messages;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace SendBigBatchWithoutImmideteDispatch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureServices((hostBuilder, services) =>
            {

            })
            .UseNServiceBus(ctx =>
            {
                var endpointConfiguration = new EndpointConfiguration("Test.Endpoint");

                var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                    .ConnectionString("Connection string");
                transport.Transactions(TransportTransactionMode.ReceiveOnly);
                transport.PrefetchCount(1000);
                endpointConfiguration.LimitMessageProcessingConcurrencyTo(64);
                endpointConfiguration.EnableInstallers();
                var recoverability = endpointConfiguration.Recoverability();
                recoverability.Immediate(i => i.NumberOfRetries(2));

                return endpointConfiguration;
            });
    }



    public class TestHandler : IHandleMessages<TestCommand>
    {
        public async Task Handle(TestCommand message, IMessageHandlerContext context)
        {
            var command = new TestCommand2 { Id = message.Id.ToString() };

            var tasks = new List<Task>();
            for (int i = 0; i < 100000; i++)
            {
                tasks.Add(context.SendLocal(command));
            }

            await Task.WhenAll(tasks);
        }
    }

    public class MessageDrain : IHandleMessages<TestCommand2>
    {
        public async Task Handle(TestCommand2 message, IMessageHandlerContext context)
        {
            await Task.CompletedTask;
        }
    }
}
