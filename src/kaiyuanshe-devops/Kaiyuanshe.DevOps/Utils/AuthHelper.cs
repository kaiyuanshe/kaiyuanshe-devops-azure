using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Kaiyuanshe.DevOps.Utils
{
    internal class AuthHelper
    {
        /// <summary>
        /// Calculate the authorization header.
        /// </summary>
        /// <param name="requestUrl">Complete request URL with scheme, host, path and queries</param>
        /// <param name="requestTime">UTC request time with format yyyy-MM-dd HH:mm:ss.</param>
        /// <param name="keyID">The API key ID.</param>
        /// <param name="keyValue">The API key value.</param>
        /// <param name="httpMethod">Http method in upper case</param>
        /// <returns>Calculated authorization header</returns>
        public static string CalculateAuthorizationHeader(string requestUrl, string requestTime, string keyID, string keyValue, string httpMethod)
        {
            Uri requestUri = new Uri(requestUrl);

            StringBuilder hashContentBuilder = new StringBuilder();
            hashContentBuilder.Append(requestUri.AbsolutePath.ToLowerInvariant());
            hashContentBuilder.Append("\r\n");

            var queryStrings = HttpUtility.ParseQueryString(requestUri.Query);
            var sortedParameterNames = queryStrings.AllKeys.ToList();
            sortedParameterNames.Sort((q1, q2) => string.Compare(q1, q2));
            var result = string.Join(", ", sortedParameterNames.Select(p => string.Format("{0}:{1}", p, queryStrings[p])).ToArray());
            if (!string.IsNullOrEmpty(result))
            {
                hashContentBuilder.Append(result);
                hashContentBuilder.Append("\r\n");
            }

            hashContentBuilder.Append(requestTime);
            hashContentBuilder.Append("\r\n");
            hashContentBuilder.Append(httpMethod.ToUpper());
            string hashContent = hashContentBuilder.ToString();

            using (HMACSHA256 myhmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(keyValue)))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(hashContent);
                byte[] hashedValue = myhmacsha256.ComputeHash(byteArray);

                string sbinary = string.Empty;
                for (int i = 0; i < hashedValue.Length; i++)
                {
                    sbinary += hashedValue[i].ToString("X2");
                }

                return string.Format("AzureCDN {0}:{1}", keyID, sbinary);
            }
        }
    }
}
