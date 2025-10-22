using Piloteer.Commands;
using Piloteer.PowerPlatform.Commands;
using Piloteer.PowerPlatform.Metadata;
using Reqnroll;

namespace Piloteer.PowerPlatform.Steps
{
    [Binding]
    public class CreateRecordSteps
    {
        private readonly IPowerPlatformCommandFactory _commandFactory;
        private readonly ICommandProcessor _commandProcessor;

        public CreateRecordSteps(IPowerPlatformCommandFactory commandFactory, ICommandProcessor commandProcessor)
        {
            _commandFactory = commandFactory;
            _commandProcessor = commandProcessor;
        }

        [Given(@"a ([^\s]+) named ([^\s]+) with the following values")]
        [Given(@"an ([^\s]+) named ([^\s]+) with the following values")]
        [When(@"a ([^\s]+) named ([^\s]+) is created with the following values")]
        [When(@"an ([^\s]+) named ([^\s]+) is created with the following values")]
        public async Task CreateNewRecord(string entityName, string alias, Table attributeTable)
        {
            var attributes = attributeTable.CreateSet<UnparsedAttribute>();
            await _commandProcessor.ExecuteCommandAsync(_commandFactory.GetCreateRecordCommand(entityName, alias, attributes));
        }
    }
}
