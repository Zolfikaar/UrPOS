using System.Collections.Generic;
using UrPOS.Core.Entities.Base;


namespace UrPOS.Core.Entities
{
    public class PurchaseInvoice : BaseInvoice
    {
        public int SupplierId { get; set; }
        public decimal Tax { get; set; }
        public List<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();
    }

    public class PurchaseInvoiceItem : BaseInvoiceItem
    {
        public long PurchaseInvoiceId { get; set; }
        public decimal CostPrice { get; set; } // سعر الشراء من المندوب
    }
}
