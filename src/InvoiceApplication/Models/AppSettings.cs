using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class AppSettings
    {
        //ID
        public int ID { get; set; }

        //Company Info
        public string CompanyName { get; set; }
        public string RegNumber { get; set; }
        public string FinancialNumber { get; set; }
        public string EUFinancialNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }

        //Company Contact Info
        public string Website { get; set; }
        public string Phone { get; set; }

        //Company Email Info
        public string Email { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
        public int Port { get; set; }

        //Company Address Info
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        //Logo
        public string Logo { get; set; }
        public bool UseLogo { get; set; }

        //Invoice Prefix
        public string Prefix { get; set; }
    }
}
