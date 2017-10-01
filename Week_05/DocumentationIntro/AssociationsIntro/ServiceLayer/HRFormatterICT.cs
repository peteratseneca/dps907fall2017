using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace AssociationsIntro.ServiceLayer
{
    // Attention 16 - This is a "link" class that describes a link relation

    // All tokens are lower-case, to conform to web standards

    /// <summary>
    /// A hypermedia link
    /// </summary>
    public class link
    {
        /// <summary>
        /// Rel - relation
        /// </summary>
        public string rel { get; set; }

        /// <summary>
        /// Href - hypermedia reference
        /// </summary>
        public string href { get; set; }

        // New added properties...

        // The null value handling issue is controversial
        // Attributes were used here to make the result look nicer (without null-valued properties)
        // However, read these...
        // StackOverflow - http://stackoverflow.com/questions/10150312/removing-null-properties-from-json-in-mvc-web-api-4-beta
        // CodePlex - http://aspnetwebstack.codeplex.com/workitem/243

        /// <summary>
        /// ContentType - internet media type, for content negotiation
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        /// <summary>
        /// Method - HTTP method(s) which can be used
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string methods { get; set; }

        /// <summary>
        /// Title - human-readable title label
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string title { get; set; }
    }

}