using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_Invoice.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }

        public int DebtorID { get; set; }

        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Invoice Date")]
        public DateTime CreatedOn { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expiration Date")]
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("DebtorID")]
        public virtual Debtor Debtor { get; set; }

        public virtual ICollection<InvoiceItem> Items { get; set; }
    }
}
