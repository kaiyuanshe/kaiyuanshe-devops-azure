using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Uri CdnKeyVaultBaseUri
        {
            get
            {
                string uri = Environment.GetEnvironmentVariable("CDN_VAULT_BASEURI");
                return new Uri(uri);
            }
        }

        public static IEnumerable<string> CdnDomains
        {
            get
            {
                var domains = Environment.GetEnvironmentVariable("CDN_DOMAINS");
                if (domains != null)
                {
                    return domains.Split(',', ';')
                        .Where(d => !string.IsNullOrWhiteSpace(d))
                        .Select(d => d.Trim());
                }

                return new List<string>();
            }
        }
    }
}
