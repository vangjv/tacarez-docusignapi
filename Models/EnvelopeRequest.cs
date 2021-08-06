using System;
using System.Collections.Generic;
using System.Text;

namespace TacarEZDocusignAPI.Models
{
    public class EnvelopeRequest
    {
        public string featureName { get; set; }
        public string mapLink { get; set; }
        public string linkText { get; set; }
        public string mapVersion { get; set; }
        public string changeNotes { get; set; }
        public List<EnvelopeRecipient> recipients { get; set; }
    }

    public class EnvelopeRecipient
    {
        public string name { get; set; }
        public string email { get; set; }
    }
}
