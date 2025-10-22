using Piloteer.PowerPlatform.Connectivity;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Piloteer.PowerPlatform.UnitTests.Connectivity
{
    [Binding]
    [Scope(Feature = "Connectivity")]
    public class ConnectivitySteps
    {
        private readonly IAppSettingsProvider _provider;
        private readonly IPowerPlatformConnection _connection;

        private IDataverseService? _serviceClient;

        public ConnectivitySteps(IAppSettingsProvider provider, IPowerPlatformConnection connection)
        {
            _provider = provider;
            _connection = connection;
        }

        [When(@"I connect to dataverse")]
        public void ConnectToDataverse()
        {
            _serviceClient = _connection.Service;
        }

        [Then(@"I should be logged in with the app registration from the app config")]
        public async Task AssertConnectionWithAppRegistration()
        {
            Assert.NotNull(_serviceClient);

            var expectedClientId = Guid.Parse(_provider.GetRequiredAppSettingsValue("PowerPlatform:ClientId"));
            var whoAmI = await _serviceClient.ExecuteAsync<WhoAmIResponse>(new WhoAmIRequest());
            var user = await _serviceClient.RetrieveAsync("systemuser", whoAmI.UserId, new ColumnSet("applicationid"));
            Assert.Equal(expectedClientId, user.GetAttributeValue<Guid>("applicationid"));
        }

        [Then(@"I should be logged in with userId '(.*)'")]
        public async Task AssetConnectionWithUser(Guid userId)
        {
            Assert.NotNull(_serviceClient);
            Assert.Equal(userId, (await _serviceClient.GetUserSettingsAsync()).UserId);
        }
    }
}
