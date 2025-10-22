using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Piloteer.PowerPlatform.Metadata
{
    public interface IEntityParser
    {
        (AttributeMetadata AttributeMetadata, object? ParsedValue) ParseAttribute(EntityMetadata metadata, UnparsedAttribute attribute, TimeZoneInfo timeZone, IAliasedRecordCache recordCache);
        Entity ParseEntity(EntityMetadata metadata, IEnumerable<UnparsedAttribute> attributes, TimeZoneInfo timeZone, IAliasedRecordCache recordCache);
    }
}
