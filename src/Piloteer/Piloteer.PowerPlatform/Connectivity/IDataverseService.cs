using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Piloteer.PowerPlatform.Models;

namespace Piloteer.PowerPlatform.Connectivity
{
    public interface IDataverseService : IDisposable
    {
        Task<UserSettings> GetUserSettingsAsync();

        Task<Guid> CreateAsync(Entity entity);
        Task DeleteAsync(EntityReference entityReference);
        Task DeleteAsync(Entity entity);
        Task<T> ExecuteAsync<T>(OrganizationRequest request) where T : OrganizationResponse;
        Task<Entity> RetrieveAsync(string entityLogicalName, Guid userId, ColumnSet columnSet);
        Task<Entity> RetrieveAsync(EntityReference entityReference, ColumnSet columnSet);
        Task<EntityCollection> RetrieveMultipleAsync(QueryExpression query);
        Task UpdateAsync(Entity entity);
    }
}
