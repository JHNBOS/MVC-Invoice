using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication
{

    public class RootObject
    {
        [JsonProperty("AppSettings")]
        public AppSettings Data { get; set; }
    }

    public class AppSettings
    {
        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("SMTP")]
        public string SMTP { get; set; }

        [JsonProperty("Port")]
        public int Port { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Website")]
        public string Website { get; set; }

        [JsonProperty("Logo")]
        public string Logo { get; set; }

        [JsonProperty("UseLogo")]
        public bool UseLogo { get; set; }

        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("PostalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("TaxNumber")]
        public string TaxNumber { get; set; }

        [JsonProperty("CompanyNumber")]
        public string CompanyNumber { get; set; }
    }
}
