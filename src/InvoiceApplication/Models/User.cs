using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string AccountType { get; set; }
    }
}
