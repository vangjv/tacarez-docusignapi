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
            EnvelopeRequest envelopeRequest = JsonConvert.DeserializeObject<EnvelopeRequest>(requestBody);
            //dynamic envelopeRequest = JsonConvert.DeserializeObject(requestBody);
            if (envelopeRequest.recipients.Count < 1)
            {
                return new BadRequestObjectResult("Invalid request");
            }
            string accessToken = Utility.GetAccessToken();
            //var envelopeId = SendEmailEnvelope.SendEnvelopeViaEmail(envelopeRequest, accessToken, Utility.GetEnvironmentVariable("basePath"), Utility.GetEnvironmentVariable("accountId"));
            var envelopeId = SendStakeholderReview.SendStakeHolderReviewEmails(envelopeRequest, accessToken, Utility.GetEnvironmentVariable("basePath"), Utility.GetEnvironmentVariable("accountId"));
            return new OkObjectResult(envelopeId);
        }
    }
}
