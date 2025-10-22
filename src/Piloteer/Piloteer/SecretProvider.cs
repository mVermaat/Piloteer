using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Piloteer
{
    internal class SecretProvider : ISecretProvider
    {
        private readonly IAppSettingsProvider _appSettingsProvider;

        private Lazy<TokenCredential> _tokenCredential;

        public SecretProvider(IAppSettingsProvider appSettingsProvider)
        {
            _appSettingsProvider = appSettingsProvider;
            _tokenCredential = new Lazy<TokenCredential>(GetNewToken);
        }

        public string GetSecret(string name)
        {
            var kvUri = new Uri($"https://{_appSettingsProvider.GetRequiredAppSettingsValue("KeyVault:Name")}.vault.azure.net");
            var client = new SecretClient(kvUri, _tokenCredential.Value);

            var secret = client.GetSecret(name);
            return secret.Value.Value;
        }

        private ClientSecretCredential GetNewToken()
        {
            var clientId = _appSettingsProvider.GetRequiredAppSettingsValue("KeyVault:ClientId");
            var tenantId = _appSettingsProvider.GetRequiredAppSettingsValue("General:TenantId");
            var clientSecret = _appSettingsProvider.GetRequiredAppSettingsValue("KeyVault:ClientSecret");
            return new ClientSecretCredential(tenantId, clientId, clientSecret);
        }
    }
}
