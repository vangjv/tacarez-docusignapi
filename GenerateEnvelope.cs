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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string accessToken = Utility.GetAccessToken();
            var envelopeId = SendEmailEnvelope.SendEnvelopeViaEmail("jonathan.vang@gmail.com", "jonathan vang", accessToken, Utility.GetEnvironmentVariable("basePath"), Utility.GetEnvironmentVariable("accountId"));

            Console.WriteLine("Hello World!");

            return new OkObjectResult(envelopeId);
        }
    }
}
