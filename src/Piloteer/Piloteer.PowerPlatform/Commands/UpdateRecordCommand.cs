using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Piloteer.Commands;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Metadata;

namespace Piloteer.PowerPlatform.Commands
{
    public class UpdateRecordCommand : MixedCommand
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly string _recordAlias;
        private readonly IEnumerable<UnparsedAttribute> _attributes;

        public UpdateRecordCommand(
            IPowerPlatformTestingContext commandContext,
            string recordAlias,
            IEnumerable<UnparsedAttribute> attributes) 
            : base(commandContext.AppSettingsProvider)
        {
            _context = commandContext;
            _recordAlias = recordAlias;
            _attributes = attributes;
        }

        protected override async Task ExecuteApiAsync()
        {
            var recordRef = _context.AliasedRecordCache.GetRequired(_recordAlias);
            var metadata = await _context.MetadataCache.GetEntityMetadataAsync(_context.PowerPlatformConnection, recordRef.LogicalName);           
            var entity = _context.EntityParser.ParseEntity(metadata, _attributes, (await _context.PowerPlatformConnection.Service.GetUserSettingsAsync()).TimeZoneInfo, _context.AliasedRecordCache);
           
            entity.Id = recordRef.Id;
            await _context.PowerPlatformConnection.Service.UpdateAsync(entity);
        }

        protected override Task ExecuteBrowserAsync()
        {
            throw new NotImplementedException();
        }
    }
}
