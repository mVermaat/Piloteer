using Microsoft.Xrm.Sdk;

namespace Piloteer.PowerPlatform
{
    public interface IAliasedRecordCache
    {
        Task AddAsync(string alias, Entity entity);
        Task AddAsync(string alias, EntityReference reference);
        EntityReference GetRequired(string alias);
        EntityReference? GetOptional(string alias);
        void Remove(string alias);
        Task UpsertAsync(string alias, Entity entity);
        Task UpsertAsync(string alias, EntityReference entityReference);
    }
}
