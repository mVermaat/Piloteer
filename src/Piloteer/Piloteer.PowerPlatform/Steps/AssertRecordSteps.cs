using Piloteer.Commands;
using Piloteer.PowerPlatform.Commands;
using Piloteer.PowerPlatform.Metadata;
using Reqnroll;

namespace Piloteer.PowerPlatform.Steps
{
    [Binding]
    public class AssertRecordSteps
    {
        private readonly IPowerPlatformCommandFactory _commandFactory;
        private readonly ICommandProcessor _commandProcessor;

        public AssertRecordSteps(IPowerPlatformCommandFactory commandFactory, ICommandProcessor commandProcessor)
        {
            _commandFactory = commandFactory;
            _commandProcessor = commandProcessor;
        }

        [Then(@"a ([^\s]+) exists with the following values")]
        [Then(@"an ([^\s]+) exists with the following values")]
        public async Task CreateNewRecord(string entityName, Table attributeTable)
        {
            var attributes = attributeTable.CreateSet<UnparsedAttribute>();
            await _commandProcessor.ExecuteCommandAsync(_commandFactory.GetGetRecordsCommand(entityName, attributes));
        }
    }
}
