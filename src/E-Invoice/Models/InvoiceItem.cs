using System.ComponentModel.DataAnnotations;

namespace E_Invoice.Models
{
    public class InvoiceItem
    {
        [Key]
        public int ItemID { get; set; }

        public int InvoiceID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }


        public virtual Invoice Invoice { get; set; }
        public virtual Product Product { get; set; }
    }
}
