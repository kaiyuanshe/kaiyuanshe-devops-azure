using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Kaiyuanshe.DevOps.Utils
{
    internal static class CertificateHelper
    {
        internal static async Task<X509Certificate2> GetServerCertificateAsync(string url)
        {
            X509Certificate2 certificate = null;
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, cert, __, ___) =>
                {
                    certificate = new X509Certificate2(cert.GetRawCertData(), default(string), X509KeyStorageFlags.Exportable);
                    return true;
                }
            };

            var httpClient = new HttpClient(httpClientHandler);
            await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

            return certificate;
        }

        internal static bool Export(this X509Certificate2 certificate, out string publicCert, out string privateKey)
        {
            publicCert = null;
            privateKey = null;

            byte[] certificateBytes = certificate.RawData;
            X509Chain chain = new X509Chain();
            bool build = chain.Build(certificate);
            if (!build || chain.ChainElements.Count <= 1)
            {
                return false;
            }
            X509Certificate2 issuer = chain.ChainElements[1].Certificate;
            char[] leafPem = PemEncoding.Write("CERTIFICATE", certificateBytes);
            char[] issuerPem = PemEncoding.Write("CERTIFICATE", issuer.RawData);
            publicCert = new string(leafPem) + "\n" + new string(issuerPem);

            RSA key = certificate.GetRSAPrivateKey();
            byte[] privKeyBytes = key.ExportPkcs8PrivateKey();
            char[] privKeyPem = PemEncoding.Write("PRIVATE KEY", privKeyBytes);
            privateKey = new string(privKeyPem);

            return true;
        }
    }
}
