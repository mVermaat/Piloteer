using Piloteer.PowerPlatform.Metadata;

namespace Piloteer.PowerPlatform.Commands
{
    public interface IPowerPlatformCommandFactory
    {
        CreateRecordCommand GetCreateRecordCommand(string entityLogicalName, string alias, IEnumerable<UnparsedAttribute> attributes);
        GetRecordsCommand GetGetRecordsCommand(string entityLogicalName, IEnumerable<UnparsedAttribute> criteria);
        SetModelAppCommand GetSetModelAppCommand(string modelAppId);
        UpdateRecordCommand GetUpdateRecordCommand(string alias, IEnumerable<UnparsedAttribute> attributes);
    }
}
