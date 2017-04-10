using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InvoiceApplication.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Display(Name = "Tax Percentage")]
        public int TaxPercentage { get; set; }

        public virtual List<InvoiceItem> InvoiceItems { get; set; }
    }

}



