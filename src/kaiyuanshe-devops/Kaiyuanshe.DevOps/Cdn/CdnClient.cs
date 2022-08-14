using Kaiyuanshe.DevOps.Cdn.Models;
using Kaiyuanshe.DevOps.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
        private readonly string cdnBaseUri;

        public CdnClient(ILogger logger, string subscriptionId, string keyId, string keyValue)
        {
            this.logger = logger;
            SubscriptionId = subscriptionId;
            KeyId = keyId;
            KeyValue = keyValue;
            cdnBaseUri = "https://restapi.cdn.azure.cn/subscriptions/" + SubscriptionId + "/";
        }

        public async Task<IEnumerable<Endpoint>> ListEndpoints()
        {
            logger.LogInformation("ListEndpoints:");
            string url = cdnBaseUri + "endpoints?apiVersion=1.0";
            var resp = await Get(url);
            return JsonConvert.DeserializeObject<Endpoint[]>(resp);
        }

        public async Task<UploadCertificateResponse> UploadCertificate(string certName, string pubCert, string priKey)
        {
            logger.LogInformation($"UploadCertificate: {certName}");

            var req = new UploadCertificateRequest
            {
                CertificateName = certName,
                PublicCertificate = pubCert,
                PrivateKey = priKey,
                Format = "Pem"
            };

            string url = cdnBaseUri + "https/certificates?apiVersion=1.0";
            var resp = await Post(url, req);
            return JsonConvert.DeserializeObject<UploadCertificateResponse>(resp);
        }

        public async Task<HttpsBindingResponse> BindHttps(string endpointId, string certificateId)
        {
            var req = new HttpsBindingRequest
            {
                CertificateId = certificateId,
                EndpointId = endpointId,
                AutoHttpsRedirect = true,
                OriginProtocol = "FollowRequest"
            };

            string url = cdnBaseUri + "https/bindings?apiVersion=1.0";
            var resp = await Post(url, req);
            return JsonConvert.DeserializeObject<HttpsBindingResponse>(resp);
        }

        private async Task<string> Get(string url)
        {
            string date = DateTime.UtcNow.ToString(DateFormat);
            string auth = AuthHelper.CalculateAuthorizationHeader(url, date, KeyId, KeyValue, "GET");

            using HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add(DateHeader, date);
            http.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), auth);

            var resp = await http.GetStringAsync(url);
            logger.LogInformation(resp);
            return resp;
        }

        private async Task<string> Post<T>(string url, T data)
        {
            string date = DateTime.UtcNow.ToString(DateFormat);
            string auth = AuthHelper.CalculateAuthorizationHeader(url, date, KeyId, KeyValue, "POST");

            using HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add(DateHeader, date);
            http.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), auth);
            http.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), "application/json");

            var resp = await http.PostAsJsonAsync(url, data);
            var respContent = await resp.Content.ReadAsStringAsync();
            logger.LogInformation(respContent);

            return respContent;
        }
    }
}
