using NServiceBus;

namespace Messages
{
    public class TestCommand : ICommand
    {
        public int Id { get; set; }
    }
}