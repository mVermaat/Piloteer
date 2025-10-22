using Piloteer.Commands;
using Piloteer.PowerPlatform.Commands;
using Reqnroll;

namespace Piloteer.PowerPlatform.Steps
{
    [Binding]
    public class SetupSteps
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IPowerPlatformCommandFactory _commandFactory;

        public SetupSteps(ICommandProcessor commandProcessor, IPowerPlatformCommandFactory commandFactory)
        {
            _commandProcessor = commandProcessor;
            _commandFactory = commandFactory;
        }

        [Given(@"the model driven app ([^\s]+)")]
        public async Task CreateNewRecord(string appUniqueName)
        {
            await _commandProcessor.ExecuteCommandAsync(_commandFactory.GetSetModelAppCommand(appUniqueName));
        }
    }
}
