using Piloteer.Commands;

namespace Piloteer.UnitTests.Commands
{
    internal class TestBrowserCommand : BrowserOnlyCommand
    {
        public TestBrowserCommand(IAppSettingsProvider appSettingsProvider)
            : base(appSettingsProvider)
        {
        }

        public bool Executed { get; set; }

        protected override Task ExecuteBrowserAsync()
        {
            Executed = true;
            return Task.CompletedTask;
        }
    }
}
