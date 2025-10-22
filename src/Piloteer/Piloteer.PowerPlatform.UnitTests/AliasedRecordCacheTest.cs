using Microsoft.Xrm.Sdk;
using NSubstitute;
using NSubstitute.Extensions;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Metadata;
using Piloteer.UnitTests.Mocks;

namespace Piloteer.PowerPlatform.UnitTests
{
    public class AliasedRecordCacheTest
    {
        private readonly IPowerPlatformConnection _connection;
        private readonly IMetadataCache _metadataCache;
        private readonly ReqnrollOutputHelperMock _logger;
        private AliasedRecordCache _cache;

        public AliasedRecordCacheTest()
        {
            _connection = Substitute.For<IPowerPlatformConnection>();
            _metadataCache = Substitute.For<IMetadataCache>();
            _metadataCache.GetEntityMetadataAsync(_connection, Arg.Any<string>(), Arg.Any<Microsoft.Xrm.Sdk.Metadata.EntityFilters>())
                .Returns(new Microsoft.Xrm.Sdk.Metadata.EntityMetadata
                {
                    LogicalName = "account",
                });
            _logger = new();

            _cache = new(_metadataCache, _connection, _logger);
        }

        [Fact]
        public async Task TestAddAndGetRecord()
        {
            // Arrange
            var alias = "testAlias";
            var entityReference = new EntityReference("account", Guid.NewGuid());

            // Act
            await _cache.AddAsync(alias, entityReference);
            var retrievedRecord = _cache.GetRequired(alias);

            // Assert
            Assert.NotNull(retrievedRecord);
            Assert.Equal(entityReference, retrievedRecord);
        }

        [Fact]
        public void TestGettingNonExistantRecordThrowsErrorIfMustExist()
        {
            // Arrange
            var alias = "nonExistantAlias";
            // Act & Assert
            Assert.Throws<TestExecutionException>(() => _cache.GetRequired(alias));
        }

        [Fact]
        public void TestGettingNonExistantRecordReturnsNullIfOptional()
        {
            // Arrange
            var alias = "nonExistantAlias";

            // Act
            var retrievedRecord = _cache.GetOptional(alias);

            // Assert
            Assert.Null(retrievedRecord);
        }

        [Fact]
        public async Task TestUpsertRecord()
        {
            // Arrange
            var alias = "testAlias";
            var alias2 = "testAlias2";
            var entityReference = new EntityReference("account", Guid.NewGuid());
            var entityReference2 = new EntityReference("account", Guid.NewGuid());

            // Act
            await _cache.UpsertAsync(alias, entityReference);
            await _cache.UpsertAsync(alias2, entityReference);
            await _cache.UpsertAsync(alias2, entityReference2);
            var retrievedRecord = _cache.GetRequired(alias);
            var retrievedRecord2 = _cache.GetRequired(alias2);

            // Assert
            Assert.NotNull(retrievedRecord);
            Assert.Equal(entityReference, retrievedRecord);
            Assert.NotNull(retrievedRecord2);
            Assert.Equal(entityReference2, retrievedRecord2);
        }
    }
}
