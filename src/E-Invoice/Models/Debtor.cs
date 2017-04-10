using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Invoice.Models
{
    public class Debtor
    {
        [Key]
        public int DebtorID { get; set; }

        [Display(Name = "First Name")]
        [StringLength(maximumLength: 70, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 70 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 100 characters")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Bank Account Number")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "Bank Account Number must be between 5 and 100 characters")]
        public string BankAccount { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 100 characters")]
        public string Address { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(maximumLength: 12, ErrorMessage = "Postal Code cannot be longer than 12 characters")]
        public string PostalCode { get; set; }

        [StringLength(maximumLength: 100, ErrorMessage = "City cannot be longer than 100 characters")]
        public string City { get; set; }

        [StringLength(maximumLength: 100, ErrorMessage = "Country cannot be longer than 100 characters")]
        public string Country { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
