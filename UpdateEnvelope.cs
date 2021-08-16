using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TacarEZDocusignAPI.Models;
using TacarEZDocusignAPI.Helpers;

namespace TacarEZDocusignAPI
{
    public static class UpdateEnvelope
    {
        [FunctionName("UpdateEnvelope")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EnvelopeRequest envelopeRequest = JsonConvert.DeserializeObject<EnvelopeRequest>(requestBody);
            //dynamic envelopeRequest = JsonConvert.DeserializeObject(requestBody);
            if (envelopeRequest.recipients.Count < 1)
            {
                return new BadRequestObjectResult("Invalid request");
            }
            string accessToken = Utility.GetAccessToken();
            //var envelopeId = SendEmailEnvelope.SendEnvelopeViaEmail(envelopeRequest, accessToken, Utility.GetEnvironmentVariable("basePath"), Utility.GetEnvironmentVariable("accountId"));
            var envelopeId = UpdateEnvelopeHelper.UpdateEnvelopeWith2Documents(envelopeRequest, accessToken, Utility.GetEnvironmentVariable("basePath"), Utility.GetEnvironmentVariable("accountId"), "463362dc-b520-44f8-9350-62fd94c23914");
            return new OkObjectResult(envelopeId);
        }
    }
}
