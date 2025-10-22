using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Piloteer.PowerPlatform.Models;

namespace Piloteer.PowerPlatform.Connectivity
{
    internal class DataverseService : IDataverseService
    {
        private readonly ServiceClient _serviceClient;
        private UserSettings? _userSettings;

        private bool _disposedValue;


        public DataverseService(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public Task<Guid> CreateAsync(Entity entity)
        {
            return _serviceClient.CreateAsync(entity);
        }

        public Task DeleteAsync(EntityReference entityReference)
        {
            return _serviceClient.DeleteAsync(entityReference.LogicalName, entityReference.Id);
        }

        public Task DeleteAsync(Entity entity)
        {
            return _serviceClient.DeleteAsync(entity.LogicalName, entity.Id);
        }

        public async Task<T> ExecuteAsync<T>(OrganizationRequest request)
           where T : OrganizationResponse
        {
            return (T)await _serviceClient.ExecuteAsync(request);
        }

        public Task<Entity> RetrieveAsync(string entityLogicalName, Guid userId, ColumnSet columnSet)
        {
            return _serviceClient.RetrieveAsync(entityLogicalName, userId, columnSet);
        }

        public Task<Entity> RetrieveAsync(EntityReference entityReference, ColumnSet columnSet)
        {
            return _serviceClient.RetrieveAsync(entityReference.LogicalName, entityReference.Id, columnSet);
        }

        public Task<EntityCollection> RetrieveMultipleAsync(QueryExpression query)
        {
            return _serviceClient.RetrieveMultipleAsync(query);
        }

        public Task UpdateAsync(Entity entity)
        {
            return _serviceClient.UpdateAsync(entity);
        }

        internal void ImpersonateUser(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                _serviceClient.CallerId = Guid.Empty;
                _userSettings = null;
                return;
            }

            var userQuery = new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("systemuserid"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("domainname", ConditionOperator.Equal, username)
                    }
                }
            };
            var userEntity = _serviceClient.RetrieveMultiple(userQuery).Entities.FirstOrDefault();
            if (userEntity == null)
            {
                throw new InvalidOperationException($"User with username '{username}' not found.");
            }
            _serviceClient.CallerId = userEntity.GetAttributeValue<Guid>("systemuserid");
            _userSettings = null;
        }

        public async Task<UserSettings> GetUserSettingsAsync()
        {
            if (_userSettings == null)
            {
                var userId = _serviceClient.CallerId != Guid.Empty
                    ? _serviceClient.CallerId
                    : (await ExecuteAsync<WhoAmIResponse>(new WhoAmIRequest())).UserId;

                var query = new QueryExpression("usersettings")
                {
                    TopCount = 1,
                    ColumnSet = { AllColumns = true }
                };
                query.Criteria.AddCondition("systemuserid", ConditionOperator.Equal, userId);
                var settingsEntity = (await RetrieveMultipleAsync(query)).Entities[0];

                query = new QueryExpression("timezonedefinition")
                {
                    TopCount = 1
                };
                query.ColumnSet.AddColumn("standardname");
                query.Criteria.AddCondition("timezonecode", ConditionOperator.Equal, settingsEntity["timezonecode"]);
                var timeZoneEntity = (await RetrieveMultipleAsync(query)).Entities[0];
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneEntity.GetAttributeValue<string>("standardname"));

                _userSettings = new UserSettings(settingsEntity, timeZoneInfo);
            }
            return _userSettings;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _serviceClient.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
