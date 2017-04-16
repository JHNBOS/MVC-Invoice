using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InvoiceApplication.Models
{
    public class Invoice
    {
        [Key]
        [Display(Name = "Invoice Number")]
        public int InvoiceNumber { get; set; }

        [Display(Name = "Debtor")]
        public int DebtorID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Invoice Date")]
        public DateTime CreatedOn { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expiration Date")]
        public DateTime ExpirationDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Display(Name = "Save As")]
        public string Type { get; set; }

        public bool Paid { get; set; }

        public virtual Debtor Debtor { get; set; }
        public virtual List<InvoiceItem> InvoiceItems { get; set; }
    }
}
