using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class Company
    {

        [Key]
        public int CompanyID { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        [Display(Name = "Bank Account")]
        public string BankAccount { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Registration Number")]
        public string RegNumber { get; set; }

        [Required]
        [Display(Name = "National Fiscal Number")]
        public string FinancialNumber { get; set; }

        [Required]
        [Display(Name = "EU Fiscal Number")]
        public string EUFinancialNumber { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
    }
}
