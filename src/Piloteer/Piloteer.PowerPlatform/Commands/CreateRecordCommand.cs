using Piloteer.Commands;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Metadata;
using Piloteer.PowerPlatform.Playwright;
using Piloteer.PowerPlatform.Playwright.Actions;

namespace Piloteer.PowerPlatform.Commands
{
    public class CreateRecordCommand : MixedCommand
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly IPowerPlatformActionFactory _actionFactory;
        private readonly string _entityLogicalName;
        private readonly string _recordAlias;
        private readonly IEnumerable<UnparsedAttribute> _attributes;

        public CreateRecordCommand(
            IPowerPlatformTestingContext commandContext,
            IPowerPlatformActionFactory actionFactory,
            string entityLogicalName,
            string recordAlias,
            IEnumerable<UnparsedAttribute> attributes)
            : base(commandContext.AppSettingsProvider)
        {
            _context = commandContext;
            _actionFactory = actionFactory;
            _entityLogicalName = entityLogicalName;
            _recordAlias = recordAlias;
            _attributes = attributes;
        }

        protected override async Task ExecuteApiAsync()
        {
            var metadata = await _context.MetadataCache.GetEntityMetadataAsync(_context.PowerPlatformConnection, _entityLogicalName);

            var entity = _context.EntityParser.ParseEntity(metadata, _attributes, (await _context.PowerPlatformConnection.Service.GetUserSettingsAsync()).TimeZoneInfo, _context.AliasedRecordCache);
            entity.Id = await _context.PowerPlatformConnection.Service.CreateAsync(entity);

            await _context.AliasedRecordCache.AddAsync(_recordAlias, entity);
        }

        protected override async Task ExecuteBrowserAsync()
        {
            var page = await _context.BrowserSession.GetCurrentPage();
            var metadata = await _context.MetadataCache.GetEntityMetadataAsync(_context.PowerPlatformConnection, _entityLogicalName);
            var entity = _context.EntityParser.ParseEntity(metadata, _attributes, (await _context.PowerPlatformConnection.Service.GetUserSettingsAsync()).TimeZoneInfo, _context.AliasedRecordCache);

            var formData = await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetOpenRecord(new OpenFormOptions(AppSettingsProvider, entity)));
            await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetFillForm(entity, formData));
            await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSaveForm(_entityLogicalName));
            entity.Id = await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetGetRecordId());
            await _context.AliasedRecordCache.AddAsync(_recordAlias, entity);
        }
    }
}
