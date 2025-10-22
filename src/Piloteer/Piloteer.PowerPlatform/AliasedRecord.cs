using Microsoft.Xrm.Sdk;

namespace Piloteer.PowerPlatform
{
    internal class AliasedRecord
    {
        public DateTime CreatedOn { get; }
        public string Alias { get; }
        public EntityReference Record { get; set; }

        public AliasedRecord(string alias, EntityReference record)
        {
            Alias = alias;
            Record = record;
            CreatedOn = DateTime.Now;
        }
    }
}
