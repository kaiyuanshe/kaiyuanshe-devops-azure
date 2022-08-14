namespace Kaiyuanshe.DevOps.Cdn.Models
{
    public class UploadCertificateRequest
    {
        public string CertificateName { get; set; }
        public string PublicCertificate { get; set; }
        public string PrivateKey { get; set; }
        /// <summary>
        /// 证书格式， 目前仅支持Pem。
        /// </summary>
        public string Format { get; set; }
    }
}
