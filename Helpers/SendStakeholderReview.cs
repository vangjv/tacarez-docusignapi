using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TacarEZDocusignAPI.Models;
using TacarEZDocusignAPI.StakeholderTemplate;

namespace TacarEZDocusignAPI
{
    class SendStakeholderReview
    {
        public static string SendStakeHolderReviewEmails(EnvelopeRequest envelopeRequest, string accessToken, string basePath, string accountId)
        {
            EnvelopeDefinition env = MakeEnvelope(envelopeRequest);
            var apiClient = new ApiClient(basePath);
            apiClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, env);
            return results.EnvelopeId;
        }


        private static EnvelopeDefinition MakeEnvelope(EnvelopeRequest envelopeRequest)
        {
            // create the envelope definition
            EnvelopeDefinition env = new EnvelopeDefinition();
            env.EmailSubject = "TacarEZ - Your assistance has been requested to review changes to a map feature.";
            Document doc1 = new Document();
            doc1.DocumentId = "1";
            doc1.Name = "Stakeholder Review";
            DocumentHtmlDefinition htmlDef = new DocumentHtmlDefinition();
            DocumentHtmlDisplayAnchor section1 = new DocumentHtmlDisplayAnchor
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
            DocumentHtmlDisplayAnchor section2 = new DocumentHtmlDisplayAnchor
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
                    DisplayLabel = "Click to expand map data",
                    CollapsibleSettings = new DocumentHtmlCollapsibleDisplaySettings
                    {
                        ArrowOpen = "up",
                        ArrowClosed = "down"
                    }
                }
            };
            DocumentHtmlDisplayAnchor section3 = new DocumentHtmlDisplayAnchor
            {
                StartAnchor = "$$$S3$$$",
                EndAnchor = "$$$E3$$$",
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
            DocumentHtmlDisplayAnchor section4 = new DocumentHtmlDisplayAnchor
            {
                StartAnchor = "$$$S4$$$",
                EndAnchor = "$$$E4$$$",
                RemoveStartAnchor = true,
                RemoveEndAnchor = true,
                CaseSensitive = true,
                DisplaySettings = new DocumentHtmlDisplaySettings
                {
                    Display = "collapsed",
                    TableStyle = "margin-bottom: 20px;width:100%;max-width:816px;margin-left:auto;margin-right:auto;",
                    CellStyle = "text-align:left;border:solid 0px #000;margin:0px;padding:0px;",
                    DisplayLabel = "Click to expand map preview",
                    CollapsibleSettings = new DocumentHtmlCollapsibleDisplaySettings
                    {
                        ArrowOpen = "up",
                        ArrowClosed = "down"
                    }
                }
            };
            DocumentHtmlDisplayAnchor section5 = new DocumentHtmlDisplayAnchor
            {
                StartAnchor = "$$$S5$$$",
                EndAnchor = "$$$E5$$$",
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
            htmlDef.DisplayAnchors = new List<DocumentHtmlDisplayAnchor>() { section1, section2, section3, section4, section5};
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootDirectory = Path.GetFullPath(Path.Combine(binDirectory, ".."));
            htmlDef.Source = HtmlUtility.LoadHtmlFile(rootDirectory + "\\StakeholderReview.html");
            htmlDef.Source = htmlDef.Source.Replace("{{senderName}}", envelopeRequest.senderName);
            htmlDef.Source = htmlDef.Source.Replace("{{messageFromSender}}", envelopeRequest.messageFromSender);            
            htmlDef.Source = htmlDef.Source.Replace("{{mapFeatureName}}", envelopeRequest.mapFeatureName);
            htmlDef.Source = htmlDef.Source.Replace("{{originalMapFeatureLink}}", envelopeRequest.originalMapFeatureLink);
            htmlDef.Source = htmlDef.Source.Replace("{{mergeRequestLink}}", envelopeRequest.mergeRequestLink);
            htmlDef.Source = htmlDef.Source.Replace("{{mergeRequesterNotes}}", envelopeRequest.mergeRequesterNotes);
            htmlDef.Source = htmlDef.Source.Replace("{{stakeholderReviewStartDate}}", envelopeRequest.stakeholderReviewStartDate);
            htmlDef.Source = htmlDef.Source.Replace("{{hashOfMergeRequestData}}", envelopeRequest.hashOfMergeRequestData);
            htmlDef.Source = htmlDef.Source.Replace("{{rawMergeRequestData}}", envelopeRequest.rawMergeRequestData);
            htmlDef.Source = htmlDef.Source.Replace("{{mapPreviewImage}}", envelopeRequest.mapPreviewImage);
            //generate signature block
            string generatedSignatureBlock = "";
            for (int i = 0; i < envelopeRequest.recipients.Count; i++)
            {
                if (envelopeRequest.recipients[i] != null)
                {
                    string signBlockTemplate = TemplateHelper.SignatureBlock();
                    signBlockTemplate = signBlockTemplate.Replace("{{signerName}}", envelopeRequest.recipients[i].name);
                    signBlockTemplate = signBlockTemplate.Replace("{{signature}}", "/signature_" + (i + 1) + "/");
                    generatedSignatureBlock = generatedSignatureBlock + signBlockTemplate;
                }
            }
            //replace signature block in html with generated signature block
            htmlDef.Source = htmlDef.Source.Replace("{{SIGNATUREBLOCK}}", generatedSignatureBlock);
            doc1.HtmlDefinition = htmlDef;

            // The order in the docs array determines the order in the envelope
            env.Documents = new List<Document> { doc1 };

            List<Signer> signerslist = new List<Signer>();
            for (int i = 0; i < envelopeRequest.recipients.Count; i++)
            {
                if (envelopeRequest.recipients[i] !=null)
                {
                    Signer newSigner = new Signer
                    {
                        Email = envelopeRequest.recipients[i].email,
                        Name = envelopeRequest.recipients[i].name,
                        RecipientId = (i + 1).ToString()
                    };
                    newSigner.Tabs = new Tabs
                    {
                        SignHereTabs = new List<SignHere> {
                            new SignHere
                            {
                                AnchorString = "/signature_" + (i + 1) + "/",
                                AnchorUnits = "pixels",
                                AnchorYOffset = "10",
                                AnchorXOffset = "20"
                            }
                        }
                    };
                    signerslist.Add(newSigner);
                }
            }

            // Add the recipients to the envelope object
            Recipients recipients = new Recipients
            {
                Signers = signerslist,
                CarbonCopies = new List<CarbonCopy>()
                {
                    new CarbonCopy()
                    {
                        Email = envelopeRequest.senderEmail,
                        Name = envelopeRequest.senderName,
                        RecipientId = envelopeRequest.recipients.Count + 1 + ""
                    }
                }
            };
            env.Recipients = recipients;
            env.Status = "sent";
            return env;
        }

    }


}
