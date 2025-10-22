using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piloteer.Commands;
using Piloteer.PowerPlatform.Commands;
using Piloteer.PowerPlatform.Metadata;
using Reqnroll;

namespace Piloteer.PowerPlatform.Steps
{
    [Binding]
    public class UpdateRecordSteps
    {
        private readonly IPowerPlatformCommandFactory _commandFactory;
        private readonly ICommandProcessor _commandProcessor;

        public UpdateRecordSteps(IPowerPlatformCommandFactory commandFactory, ICommandProcessor commandProcessor)
        {
            _commandFactory = commandFactory;
            _commandProcessor = commandProcessor;
        }

        [When(@"([^\s]+) is updated with the following values")]
        public async Task UpdateExistingRecord(string alias, Table attributeTable)
        {
            var attributes = attributeTable.CreateSet<UnparsedAttribute>();
            await _commandProcessor.ExecuteCommandAsync(_commandFactory.GetUpdateRecordCommand(alias, attributes));
        }
    }
}
