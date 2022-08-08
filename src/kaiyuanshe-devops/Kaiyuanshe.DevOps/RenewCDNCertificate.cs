using Kaiyuanshe.DevOps.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
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

            foreach (var domain in Config.CdnDomains)
            {
                await RenewDomain(domain, logger);
            }
        }

        private static async Task RenewDomain(string domain, ILogger logger)
        {
            logger.LogInformation($"Check domain: {domain}");
            var inUseCert = await CertificateHelper.GetServerCertificateAsync($"https://{domain}");
            logger.LogInformation($"in-use certificate for domain: {inUseCert.Thumbprint}");
        }
    }
}
