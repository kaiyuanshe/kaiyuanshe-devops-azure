namespace Kaiyuanshe.DevOps.Cdn.Models
{
    /// <summary>
    /// https://docs.azure.cn/zh-cn/cdn/cdn-upload-https-certificate
    /// </summary>
    public class UploadCertificateResponse
    {
        public string CertificateId { get; set; }
        public string CertificateName { get; set; }
        public string SubscriptionId { get; set; }
        public string State { get; set; }
        public string Thumbprint { get; set; }
    }
}
