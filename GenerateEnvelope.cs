using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TacarEZDocusignAPI
{
    public static class GenerateEnvelope
    {
        [FunctionName("GenerateEnvelope")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic envelopeRequest = JsonConvert.DeserializeObject(requestBody);
            if (envelopeRequest.name == null || envelopeRequest.email == null)
            {
                return new BadRequestObjectResult("Invalid request");
            }
            string accessToken = Utility.GetAccessToken();
            var envelopeId = SendEmailEnvelope.SendEnvelopeViaEmail((string)envelopeRequest.email, (string)envelopeRequest.name, accessToken, Utility.GetEnvironmentVariable("basePath"), Utility.GetEnvironmentVariable("accountId"));

            Console.WriteLine("Hello World!");

            return new OkObjectResult(envelopeId);
        }
    }
}
