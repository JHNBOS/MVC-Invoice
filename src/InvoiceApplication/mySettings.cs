using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication
{
    public class mySettings
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
        public int Port { get; set; }

        public string Name { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
        public bool UseLogo { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        public string TaxNumber { get; set; }
        public string CompanyNumber { get; set; }
    }
}
