using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApplication.Models
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
