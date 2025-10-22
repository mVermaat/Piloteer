using Microsoft.Xrm.Sdk.Metadata;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Models;
using Piloteer.PowerPlatform.Playwright.Actions;

namespace Piloteer.PowerPlatform.Metadata
{
    public interface IMetadataCache
    {
        /// <summary>
        /// Gets the metadata for a specific entity
        /// </summary>
        /// <param name="connection">Connection to Dataverse</param>
        /// <param name="entityName">Name of the entity to get metadata for</param>
        /// <param name="filters">Filters required for the metadata</param>
        /// <returns>Metadata for an entity</returns>
        Task<EntityMetadata> GetEntityMetadataAsync(IPowerPlatformConnection connection, string entityName, EntityFilters filters = EntityFilters.Attributes);

        /// <summary>
        /// Gets metadata for a specific form
        /// </summary>
        /// <param name="testingContext">Testing context</param>
        /// <param name="actionFactory">Action factory</param>
        /// <param name="form">Form to get metadata for</param>
        /// <returns>Metadata for a form</returns>
        Task<FormMetadata> GetFormMetadataAsync(IPowerPlatformTestingContext testingContext, IPowerPlatformActionFactory actionFactory, SystemForm form);
    }
}
