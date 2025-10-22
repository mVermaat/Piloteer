using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Metadata;
using Piloteer.PowerPlatform.Playwright.Actions;

namespace Piloteer.PowerPlatform.Commands
{
    public class PowerPlatformCommandFactory : IPowerPlatformCommandFactory
    {
        private readonly IPowerPlatformTestingContext _commandContext;
        private readonly IPowerPlatformActionFactory _actionFactory;

        public PowerPlatformCommandFactory(IPowerPlatformTestingContext commandContext, IPowerPlatformActionFactory actionFactory)
        {
            _commandContext = commandContext;
            _actionFactory = actionFactory;
        }

        public virtual CreateRecordCommand GetCreateRecordCommand(string entityLogicalName, string alias, IEnumerable<UnparsedAttribute> attributes)
            => new CreateRecordCommand(_commandContext, _actionFactory, entityLogicalName, alias, attributes);

        public virtual GetRecordsCommand GetGetRecordsCommand(string entityLogicalName, IEnumerable<UnparsedAttribute> criteria)
            => new GetRecordsCommand(_commandContext, entityLogicalName, criteria);

        public virtual SetModelAppCommand GetSetModelAppCommand(string modelAppId)
            => new SetModelAppCommand(_commandContext, modelAppId);

        public virtual UpdateRecordCommand GetUpdateRecordCommand(string alias, IEnumerable<UnparsedAttribute> attributes)
           => new UpdateRecordCommand(_commandContext, alias, attributes);

    }
}
