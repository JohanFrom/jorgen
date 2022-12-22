using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using jorgen.ApplicationSettings;
using jorgen.Services.Abstract;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Options;

namespace jorgen.Services.Concrete
{
    public class KeyVaultService : IKeyVaultService
    {
        private readonly IOptionsMonitor<ApplicationOptions> _options;
        public KeyVaultService(IOptionsMonitor<ApplicationOptions> options)
        {
            _options = options;
        }

        public string GetWeatherApiKey()
        {
            SecretClientOptions options = new()
            {
                Retry =
                    {
                        Delay = TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = RetryMode.Exponential
                    }
            };

            var client = new SecretClient(new Uri(_options.CurrentValue.JorgenKeyVaultUrl), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = client.GetSecret(_options.CurrentValue.JorgenKeyVaultSecretName);

            string secretValue = secret.Value;

            return secretValue;
        }
    }
}
