using Piloteer.Commands;

namespace Piloteer.UnitTests.Commands
{
    internal class TestApiCommand : ApiOnlyCommand
    {
        public bool Executed { get; set; }

        protected override Task ExecuteApiAsync()
        {
            Executed = true;
            return Task.CompletedTask;
        }
    }
}
