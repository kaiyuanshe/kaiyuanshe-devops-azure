using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Kaiyuanshe.DevOps
{
    public static class Function1
    {
        [Function("HttpFunction")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req,
    FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("HttpFunction");
            logger.LogInformation("message logged");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Date", "Mon, 18 Jul 2016 16:06:00 GMT");
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Kaiyuanshe!!!");

            return response;
        }
    }
}
