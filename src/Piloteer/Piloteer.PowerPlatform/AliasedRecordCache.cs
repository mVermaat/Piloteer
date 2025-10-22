using System.Security.Cryptography.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Metadata;
using Reqnroll;

namespace Piloteer.PowerPlatform
{
    /// <summary>
    /// Cache of all records that were created in the specflow scenario
    /// </summary>
    [Binding]
    public class AliasedRecordCache : IAliasedRecordCache
    {
        private readonly Dictionary<string, AliasedRecord> _aliasedRecords;
        private readonly IMetadataCache _metadataCache;
        private readonly IPowerPlatformConnection _powerPlatformConnection;
        private readonly IReqnrollOutputHelper _logger;

        public AliasedRecordCache(IMetadataCache metadataCache, IPowerPlatformConnection powerPlatformConnection, IReqnrollOutputHelper logger)
        {
            _aliasedRecords = new Dictionary<string, AliasedRecord>();
            _metadataCache = metadataCache;
            _powerPlatformConnection = powerPlatformConnection;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new record to the cache
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="reference">EntityReference of the record</param>
        public async Task AddAsync(string alias, EntityReference reference)
        {
            if (string.IsNullOrEmpty(reference.Name))
            {
                var md = await _metadataCache.GetEntityMetadataAsync(_powerPlatformConnection, reference.LogicalName);
                if (!string.IsNullOrWhiteSpace(md.PrimaryNameAttribute))
                {
                    var entity = await _powerPlatformConnection.Service.RetrieveAsync(reference, new ColumnSet(md.PrimaryNameAttribute));
                    reference.Name = entity.GetAttributeValue<string>(md.PrimaryNameAttribute);
                }
            }

            _logger.WriteLine($"Adding alias {alias} to cache. {reference.Id}");
            _aliasedRecords.Add(alias, new AliasedRecord(alias, reference));
        }

        /// <summary>
        /// Adds a new record to the cache
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="entity">The record to add</param>
        public async Task AddAsync(string alias, Entity entity)
        {
            var md = await _metadataCache.GetEntityMetadataAsync(_powerPlatformConnection, entity.LogicalName);
            await AddAsync(alias, entity.ToEntityReference(md.PrimaryNameAttribute));
        }

        /// <summary>
        /// Retrieves a record from the cache and throws an error if it doesn't exist
        /// </summary>
        /// <param name="alias">Alias of the record to retrieve</param>
        /// <returns></returns>
        public EntityReference GetRequired(string alias)
        {
            var record = GetOptional(alias);

            if (record == null)
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.AliasDoesntExist, alias);

            return record;
        }

        /// <summary>
        /// Retrieves a record from the cache
        /// </summary>
        /// <param name="alias">Alias of the record to retrieve</param>
        /// <param name="mustExist">If set to true, the test fails if it doesn't exist. If set to false, it returns null if it doesn't exist</param>
        /// <returns></returns>
        public EntityReference? GetOptional(string alias)
        {
            _aliasedRecords.TryGetValue(alias, out AliasedRecord? value);
            return value?.Record;
        }

        /// <summary>
        /// Adds the record to the cache if it doesn't exist. Overwrites it if it does
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="entity">EntityReference of the record</param>
        public async Task UpsertAsync(string alias, Entity entity)
        {
            var md = await _metadataCache.GetEntityMetadataAsync(_powerPlatformConnection, entity.LogicalName);
            await UpsertAsync(alias, entity.ToEntityReference(md.PrimaryNameAttribute));
        }

        /// <summary>
        /// Adds the record to the cache if it doesn't exist. Overwrites it if it does
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="reference">EntityReference of the record</param>
        public async Task UpsertAsync(string alias, EntityReference entityReference)
        {
            if (string.IsNullOrEmpty(entityReference.Name))
            {
                var md = await _metadataCache.GetEntityMetadataAsync(_powerPlatformConnection, entityReference.LogicalName);
                if (!string.IsNullOrWhiteSpace(md.PrimaryNameAttribute))
                {
                    var entity = await _powerPlatformConnection.Service.RetrieveAsync(entityReference, new ColumnSet(md.PrimaryNameAttribute));
                    entityReference.Name = entity.GetAttributeValue<string>(md.PrimaryNameAttribute);
                }
            }

            if (_aliasedRecords.ContainsKey(alias))
            {
                _aliasedRecords[alias].Record = entityReference;
            }
            else
            {
                _aliasedRecords.Add(alias, new AliasedRecord(alias, entityReference));
            }
        }

        /// <summary>
        /// Removes a record from the cache
        /// </summary>
        /// <param name="alias">Alias of the record</param>
        public void Remove(string alias)
        {
            _aliasedRecords.Remove(alias);
        }
    }
}
