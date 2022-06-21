using System;

namespace Kaiyuanshe.DevOps
{
    internal class Config
    {
        public static string AcrEndpoint
        {
            get
            {
                return Environment.GetEnvironmentVariable("ACR_ENDPOINT");
            }
        }

        public static int AcrMinManifests
        {
            get
            {
                if (int.TryParse(Environment.GetEnvironmentVariable("ACR_MIN_MANIFESTS"), out int min) && min > 0)
                {
                    return min;
                }

                return 10;
            }
        }

        public static TimeSpan AcrManifestExpiry
        {
            get
            {
                if (int.TryParse(Environment.GetEnvironmentVariable("ACR_MANIFEST_EXPIRY_DAYS"), out int days) && days > 0)
                {
                    return TimeSpan.FromDays(days);
                }

                return TimeSpan.FromDays(90);
            }
        }
    }
}
