using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Kaiyuanshe.DevOps
{
    public class AcrCleaner
    {
        [Function("AcrCleanupTimer")]
        public static void Run([TimerTrigger("0 0 */2 * * *")] TimerInfo timerInfo,
    FunctionContext context)
        {
            var logger = context.GetLogger("AcrCleanupTimer");
            logger.LogInformation($"Function Ran. Next timer schedule = {timerInfo.ScheduleStatus.Next}");
        }
    }
}
