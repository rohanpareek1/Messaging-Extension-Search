using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsMessagingExtensionsSearch.Models
{
    public class NameData//IEnumerable
    {
        [Newtonsoft.Json.JsonProperty("LeadId")]
        public int LeadId { get; set; }

        [Newtonsoft.Json.JsonProperty("Description")]
        public string Description { get; set; }

        [Newtonsoft.Json.JsonProperty("AssignedUser")]
        public string AssignedUser { get; set; }

        [Newtonsoft.Json.JsonProperty("Contact")]
        public string Contact { get; set; }

        [Newtonsoft.Json.JsonProperty("DateReviewed")]
        public string DateReviewed { get; set; }

        [Newtonsoft.Json.JsonProperty("Converted")]
        public bool Converted { get; set; }

        [Newtonsoft.Json.JsonProperty("MarketingSource")]
        public string MarketingSource { get; set; }

        [Newtonsoft.Json.JsonProperty("ReferedBy")]
        public string ReferedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("LeadStatus")]
        public string LeadStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("OpportunityId")]
        public string OpportunityId { get; set; }

        [Newtonsoft.Json.JsonProperty("Website")]
        public string Website { get; set; }

        //public IEnumerator GetEnumerator() => throw new NotImplementedException();
    }
}
