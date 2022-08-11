using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Kaiyuanshe.DevOps.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Kaiyuanshe.DevOps
{
    public class RenewCDNCertificate
    {
        // 0 30 10 * * *: at 10:30 AM every day
        // 0 0 */4 * * *: once every 4 hours
        [Function("RenewCDNCertificateTimer")]
        public static async Task Run([TimerTrigger("0 0 */4 * * *")] TimerInfo timerInfo, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RenewCDNCertificate));
            logger.LogInformation($"RenewCDNCertificateTimer starts.");

            if (string.IsNullOrWhiteSpace(Config.CdnSubscriptionId)
                || string.IsNullOrWhiteSpace(Config.CdnKeyId)
                || string.IsNullOrWhiteSpace(Config.CdnKeyValue))
            {
                logger.LogInformation("Missing configuration. Exit.");
                return;
            }

            var secretClient = new SecretClient(Config.CdnKeyVaultBaseUri, new DefaultAzureCredential());

            foreach (var domain in Config.CdnDomains)
            {
                try
                {
                    await RenewDomain(domain, secretClient, logger);
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Failed to renew domain {domain}");
                }
            }
        }

        private static async Task RenewDomain(string domain, SecretClient secretClient, ILogger logger)
        {
            logger.LogInformation($"Check domain: {domain}");

            var inUseCert = await CertificateHelper.GetServerCertificateAsync($"https://{domain}");
            logger.LogInformation($"[{domain}]in-use certificate: {inUseCert.Thumbprint}");

            var kvCert = await DownloadCertFromKeyVault(domain, secretClient, logger);
            bool needUpload = inUseCert.Thumbprint.ToUpper() != kvCert.Thumbprint.ToUpper();
            logger.LogInformation($"[{domain}]latest certificate in key vault: {kvCert.Thumbprint}. Need upload: {needUpload}");
        }

        private static async Task<X509Certificate2> DownloadCertFromKeyVault(string domain, SecretClient secretClient, ILogger logger)
        {
            logger.LogInformation($"get latest certificate for `{domain}` from keyvault {secretClient.VaultUri}");
            string secretName = domain.Replace(".", "-");
            var secret = await secretClient.GetSecretAsync(secretName);
            return new X509Certificate2(Convert.FromBase64String(secret.Value.Value));
        }
    }
}
