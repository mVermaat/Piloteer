using Piloteer.Commands;

namespace Piloteer.UnitTests.Commands
{
    internal class TestMixedCommand : MixedCommand
    {
        public TestMixedCommand(IAppSettingsProvider appSettingsProvider)
            : base(appSettingsProvider)
        {
        }

        public bool ApiExecuted { get; private set; }
        public bool BrowserExecuted { get; private set; }

        protected override Task ExecuteApiAsync()
        {
            ApiExecuted = true;
            return Task.CompletedTask;
        }

        protected override Task ExecuteBrowserAsync()
        {
            BrowserExecuted = true;
            return Task.CompletedTask;
        }
    }
}
