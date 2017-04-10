using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Invoice.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [StringLength(maximumLength: 120, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 120 characters")]
        public string Name { get; set; }

        [StringLength(maximumLength: 300, ErrorMessage = "Description cannot be longer than 300 characters")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Tax Percentage")]
        public int TaxPercentage { get; set; }

        public virtual ICollection<InvoiceItem> Items { get; set; }
    }
}
