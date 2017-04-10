using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceMVC.Models
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

        public virtual Debtor Debtor { get; set; }
        public virtual List<InvoiceItem> InvoiceItems { get; set; }
    }
}
