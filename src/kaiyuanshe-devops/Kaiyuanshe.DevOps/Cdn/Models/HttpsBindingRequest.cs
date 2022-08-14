namespace Kaiyuanshe.DevOps.Cdn.Models
{
    public class HttpsBindingRequest
    {
        public string CertificateId { get; set; }
        public string EndpointId { get; set; }
        public string OriginProtocol { get; set; }
        public bool AutoHttpsRedirect { get; set; }
    }
}
