using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Kaiyuanshe.DevOps
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                })
                .Build();

            await host.RunAsync();
        }
    }
}
