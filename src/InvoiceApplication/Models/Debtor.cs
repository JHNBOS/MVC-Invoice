using System.ComponentModel.DataAnnotations;

namespace InvoiceApplication.Models
{
    public class Debtor
    {
        [Key]
        public int DebtorID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Personal ID Number")]
        public string IdNumber { get; set; }

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

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
