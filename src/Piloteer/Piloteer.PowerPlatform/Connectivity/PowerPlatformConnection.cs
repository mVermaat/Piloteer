using Microsoft.PowerPlatform.Dataverse.Client;
using Reqnroll;

namespace Piloteer.PowerPlatform.Connectivity
{
    [Binding]
    public class PowerPlatformConnection : IPowerPlatformConnection, IDisposable
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _url;

        private readonly Lazy<DataverseService> _service;
        private readonly IUserContextManager _userContextManager;
        private readonly IReqnrollOutputHelper _logger;

        private bool _disposedValue;

        public PowerPlatformConnection(IAppSettingsProvider appSettingsProvider, IUserContextManager userContextManager, IReqnrollOutputHelper logger)
        {
            _clientId = appSettingsProvider.GetRequiredAppSettingsValue("PowerPlatform:ClientId");
            _clientSecret = appSettingsProvider.GetRequiredAppSettingsValue("PowerPlatform:ClientSecret");
            _url = appSettingsProvider.GetRequiredAppSettingsValue("PowerPlatform:Url");

            _service = new Lazy<DataverseService>(CreateNewClient);
            _userContextManager = userContextManager;
            _userContextManager.OnUserContextChanged += ImpersonateUser; 
            _logger = logger;
        }

        public IDataverseService Service => _service.Value;

        internal void ImpersonateUser(UserProfile userProfile)
        {
            _service.Value.ImpersonateUser(userProfile.Username);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if(_service.IsValueCreated)
                        _service.Value.Dispose();

                    _userContextManager.OnUserContextChanged -= ImpersonateUser; 
                }

                _disposedValue = true;
            }
        }

        private DataverseService CreateNewClient()
        {
            _logger.WriteLine($"Connecting to Dataverse API. Url: {_url}. ClientID: {_clientId}");
            var connectionString = $"AuthType=ClientSecret;Url={_url};ClientId={_clientId};ClientSecret={_clientSecret};";

            try
            {
                var client = new ServiceClient(connectionString);

                if (!client.IsReady)
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.FailedToConnectToDataverse, client.LastException, client.LastError);

                var service = new DataverseService(client);
                return service;
            }
            catch(TestExecutionException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.FailedToConnectToDataverse, ex, ex.Message);
            }
        }

        [BeforeScenario(Order = 3000)]
        public void Load()
        {
            // Ensure the constructor registers itself with the UserContextManager
        }

       
    }
}
