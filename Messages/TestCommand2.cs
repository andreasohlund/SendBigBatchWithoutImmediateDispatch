using NServiceBus;

namespace Messages
{
    public class TestCommand2 : ICommand
    {
        public string? Id { get; set; }
    }
}