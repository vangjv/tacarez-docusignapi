using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TacarEZDocusignAPI
{
    class SendEmailEnvelope
    {
        public static string SendEnvelopeViaEmail(string signerEmail, string signerName, string accessToken, string basePath, string accountId)
        {
            EnvelopeDefinition env = MakeEnvelope(signerEmail, signerName);
            var apiClient = new ApiClient(basePath);
            apiClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, env);
            return results.EnvelopeId;
        }


        private static EnvelopeDefinition MakeEnvelope(string signerEmail, string signerName)
        {
            // create the envelope definition
            EnvelopeDefinition env = new EnvelopeDefinition();
            env.EmailSubject = "Please sign this document set. Make it TacarEZ";
            Document doc1 = new Document();
            doc1.DocumentId = "1";
            doc1.Name = "Document with link";
            DocumentHtmlDefinition htmlDef = new DocumentHtmlDefinition();
            DocumentHtmlDisplayAnchor displayAnchor1 = new DocumentHtmlDisplayAnchor
            {
                StartAnchor = "$$$S1$$$",
                EndAnchor = "$$$E1$$$",
                RemoveStartAnchor = true,
                RemoveEndAnchor = true,
                CaseSensitive = true,
                DisplaySettings = new DocumentHtmlDisplaySettings
                {
                    Display = "responsive_table_single_column",
                    TableStyle = "margin-bottom: 20px;width:100%;max-width:816px;margin-left:auto;margin-right:auto;",
                    CellStyle = "text-align:left;border:solid 0px #000;margin:0px;padding:0px;"
                }
            };
            DocumentHtmlDisplayAnchor displayAnchor2 = new DocumentHtmlDisplayAnchor
            {
                StartAnchor = "$$$S2$$$",
                EndAnchor = "$$$E2$$$",
                RemoveStartAnchor = true,
                RemoveEndAnchor = true,
                CaseSensitive = true,
                DisplaySettings = new DocumentHtmlDisplaySettings
                {
                    Display = "collapsed",
                    TableStyle = "margin-bottom: 20px;width:100%;max-width:816px;margin-left:auto;margin-right:auto;",
                    CellStyle = "text-align:left;border:solid 0px #000;margin:0px;padding:0px;",
                    DisplayLabel = "More information",
                    CollapsibleSettings = new DocumentHtmlCollapsibleDisplaySettings
                    {
                        ArrowOpen = "up",
                        ArrowClosed = "down"
                    }
                }
            };
            htmlDef.DisplayAnchors = new List<DocumentHtmlDisplayAnchor>() { displayAnchor1, displayAnchor2 };
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootDirectory = Path.GetFullPath(Path.Combine(binDirectory, ".."));
            htmlDef.Source = HtmlUtility.LoadHtmlFile(rootDirectory + "\\testdocument.html");
            htmlDef.Source = htmlDef.Source.Replace("{{signerName}}", signerName);
            htmlDef.Source = htmlDef.Source.Replace("{{signerEmail}}", signerEmail);
            doc1.HtmlDefinition = htmlDef;

            // The order in the docs array determines the order in the envelope
            env.Documents = new List<Document> { doc1 };

            // create a signer recipient to sign the document, identified by name and email
            // We're setting the parameters via the object creation
            Signer signer1 = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                RecipientId = "1",
                RoutingOrder = "1"
            };

            // routingOrder (lower means earlier) determines the order of deliveries
            // to the recipients. Parallel routing order is supported by using the
            // same integer as the order for two or more recipients.

            // create a cc recipient to receive a copy of the documents, identified by name and email
            // We're setting the parameters via setters
            //CarbonCopy cc1 = new CarbonCopy
            //{
            //    Email = ccEmail,
            //    Name = ccName,
            //    RecipientId = "2",
            //    RoutingOrder = "2"
            //};

            // Create signHere fields (also known as tabs) on the documents,
            // We're using anchor (autoPlace) positioning
            //
            // The DocuSign platform searches throughout your envelope's
            // documents for matching anchor strings. So the
            // signHere2 tab will be used in both document 2 and 3 since they
            // use the same anchor string for their "signer 1" tabs.
            SignHere signHere1 = new SignHere
            {
                AnchorString = "**signature_1**",
                AnchorUnits = "pixels",
                AnchorYOffset = "10",
                AnchorXOffset = "20"
            };

            //SignHere signHere2 = new SignHere
            //{
            //    AnchorString = "/sn1/",
            //    AnchorUnits = "pixels",
            //    AnchorYOffset = "10",
            //    AnchorXOffset = "20"
            //};

            // Tabs are set per recipient / signer
            Tabs signer1Tabs = new Tabs
            {
                SignHereTabs = new List<SignHere> { signHere1 }
            };

            signer1.Tabs = signer1Tabs;

            // Add the recipients to the envelope object
            Recipients recipients = new Recipients
            {
                Signers = new List<Signer> { signer1 },
                //CarbonCopies = new List<CarbonCopy> { cc1 }
            };
            env.Recipients = recipients;
            // Request that the envelope be sent by setting |status| to "sent".
            // To request that the envelope be created as a draft, set to "created"
            env.Status = "sent";

            return env;
        }

    }


}
