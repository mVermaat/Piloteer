using System.Collections.Concurrent;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Models;
using Piloteer.PowerPlatform.Playwright.Actions;

namespace Piloteer.PowerPlatform.Metadata
{
    public class MetadataCache : IMetadataCache
    {
        private readonly ConcurrentDictionary<string, Dictionary<EntityFilters, EntityMetadata>> _entityMetadataCache;
        private readonly ConcurrentDictionary<string, FormMetadata> _formMetadataCache;

        public MetadataCache()
        {
            _entityMetadataCache = [];
            _formMetadataCache = [];
        }

        /// <inheritdoc/>
        public async Task<EntityMetadata> GetEntityMetadataAsync(IPowerPlatformConnection connection, string entityName, EntityFilters filters = EntityFilters.Attributes)
        {
            if (!_entityMetadataCache.TryGetValue(entityName, out var metadataDic))
            {
                metadataDic = new Dictionary<EntityFilters, EntityMetadata>();
                _entityMetadataCache.TryAdd(entityName, metadataDic);
            }

            if (!metadataDic.TryGetValue(filters, out EntityMetadata? result))
            {
                var req = new RetrieveEntityRequest()
                {
                    EntityFilters = filters,
                    RetrieveAsIfPublished = true,
                    LogicalName = entityName,
                };

                result = (await connection.Service.ExecuteAsync<RetrieveEntityResponse>(req)).EntityMetadata;
                metadataDic.Add(filters, result);
            }
            return result;
        }

        /// <inheritdoc/>
        public async Task<FormMetadata> GetFormMetadataAsync(IPowerPlatformTestingContext testingContext, IPowerPlatformActionFactory actionFactory, SystemForm form)
        {
            if (!_formMetadataCache.TryGetValue(form.Id.ToString(), out var formMetadata))
            {
                var entityMetadata = await GetEntityMetadataAsync(testingContext.PowerPlatformConnection, form.EntityName, EntityFilters.Attributes);
                formMetadata = new FormMetadata(entityMetadata, form);

                await formMetadata.ParseForm(
                    testingContext.PowerPlatformConnection.Service,
                    testingContext.AppSettingsProvider,
                    await testingContext.BrowserSession.GetCurrentPage(),
                    actionFactory,
                    testingContext.ActionProcessor);


                _formMetadataCache.TryAdd(form.Id.ToString(), formMetadata);
            }

            return formMetadata;
        }
    }
}
