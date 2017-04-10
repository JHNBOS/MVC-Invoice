using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceMVC.Models
{
    public class InvoiceItem
    {
        [Key]
        public int ItemID { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceNumber { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int Amount { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Product Product { get; set; }
    }
}
