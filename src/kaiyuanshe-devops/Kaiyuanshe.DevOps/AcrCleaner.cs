using Azure.Containers.ContainerRegistry;
using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kaiyuanshe.DevOps
{
    public class AcrCleaner
    {
        // 0 30 9 * * *: at 9:30 AM every day
        // 0 0 */2 * * *: once every two hours
        [Function("AcrCleanupTimer")]
        public static async Task Run([TimerTrigger("0 0 */2 * * *")] TimerInfo timerInfo,
    FunctionContext context)
        {
            var logger = context.GetLogger("AcrCleanupTimer");
            logger.LogInformation($"AcrCleanupTimer starts.");

            var acrEndpoint = Config.AcrEndpoint;
            if (string.IsNullOrEmpty(acrEndpoint))
            {
                logger.LogWarning("ACR_ENDPOINT is not configured. Please add it to Azure Function's Appliction Settings." +
                    " Or disable this Function. Sample value: https://myacr.azurecr.cn");
                return;
            }

            var credential = new DefaultAzureCredential();
            var acr = new ContainerRegistryClient(new Uri(acrEndpoint), credential, new ContainerRegistryClientOptions
            {
                Audience = ContainerRegistryAudience.AzureResourceManagerChina
            });

            logger.LogInformation($"List repository names on {acrEndpoint}");
            foreach (var name in acr.GetRepositoryNames())
            {
                logger.LogInformation($"List manifests of repository: {name}");
                var repo = acr.GetRepository(name);
                var manifests = repo.GetAllManifestProperties()
                    .Where(m => m.CanDelete.GetValueOrDefault(true))
                    .OrderByDescending(m => m.CreatedOn)
                    .Skip(Config.AcrMinManifests)
                    .Where(m => m.CreatedOn + Config.AcrManifestExpiry < DateTimeOffset.UtcNow);
                foreach (var manifest in manifests)
                {
                    var image = acr.GetArtifact(name, manifest.Digest);
                    foreach (var tag in manifest.Tags)
                    {
                        logger.LogInformation($"Deleting tag {tag} from repository {name}");
                        await image.DeleteTagAsync(tag);
                    }
                    logger.LogInformation($"Deleting manifest {manifest.Digest} from repository {name}");
                    await image.DeleteAsync();
                }
            }
        }
    }
}
