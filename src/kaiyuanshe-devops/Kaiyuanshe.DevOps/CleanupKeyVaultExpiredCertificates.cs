using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Kaiyuanshe.DevOps.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Kaiyuanshe.DevOps
{
    internal class CleanupKeyVaultExpiredCertificates
    {
        // 0 0 8 * * *: at 8:00 AM every day
        // 0 0 */4 * * *: once every 4 hours
        [Function("CleanupExpiredCertificates")]
        public static async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RenewCDNCertificate));
            logger.LogInformation($"CleanupExpiredCertificates starts.");

            var certClient = new CertificateClient(Config.CdnKeyVaultBaseUri, new DefaultAzureCredential());
            var certs = certClient.GetPropertiesOfCertificatesAsync();
            await certs.ForEach(async (prop) =>
            {
                if (prop.Tags != null && prop.Tags.ContainsKey("Issuer") && prop.Tags["Issuer"] == "Acmebot")
                {
                    logger.LogInformation($"Certificate {prop.Name} is managed by Acembot, skipped.");
                    return;
                }

                if (!prop.ExpiresOn.HasValue || prop.ExpiresOn > DateTimeOffset.UtcNow)
                {
                    logger.LogInformation($"Certificate {prop.Name} is not expired, skipped.");
                    return;
                }

                if (Guid.TryParse(prop.Name, out Guid result))
                {
                    logger.LogInformation($"Deleting certificate: {prop.Name}");
                    await certClient.StartDeleteCertificateAsync(prop.Name);
                }
                else
                {
                    logger.LogInformation($"Certificate {prop.Name} is not a valid guid, skipped.");
                    return;
                }
            });
        }
    }
}
