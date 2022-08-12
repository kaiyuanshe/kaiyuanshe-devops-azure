using Kaiyuanshe.DevOps.Cdn.Models;
using Kaiyuanshe.DevOps.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kaiyuanshe.DevOps.Cdn
{
    public class CdnClient
    {
        const string DateFormat = "yyyy-MM-dd HH:mm:ss";
        const string DateHeader = "x-azurecdn-request-date";

        ILogger logger;
        public string SubscriptionId { get; }
        public string KeyId { get; }
        public string KeyValue { get; }

        public CdnClient(ILogger logger, string subscriptionId, string keyId, string keyValue)
        {
            this.logger = logger;
            SubscriptionId = subscriptionId;
            KeyId = keyId;
            KeyValue = keyValue;
        }

        public async Task<IEnumerable<Endpoint>> ListEndpoints()
        {
            logger.LogInformation("ListEndpoints:");
            string url = $"https://restapi.cdn.azure.cn/subscriptions/{SubscriptionId}/endpoints?apiVersion=1.0";
            string date = DateTime.UtcNow.ToString(DateFormat);
            string auth = AuthHelper.CalculateAuthorizationHeader(url, date, KeyId, KeyValue, "GET");

            using HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add(DateHeader, date);
            http.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), auth);
            var resp = await http.GetStringAsync(url);
            logger.LogInformation(resp);

            return JsonConvert.DeserializeObject<Endpoint[]>(resp);
        }
    }
}
