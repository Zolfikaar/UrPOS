using System.Collections.Generic;
using UrPOS.Core.Entities.Base;

namespace UrPOS.Core.Entities
{
    public class SalesInvoice : BaseInvoice
    {
        public decimal Discount { get; set; }
        public int CustomerId { get; set; } // للمستقبل إذا دعمنا عملاء الآجل
        public List<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>(); 
    }

    public class SalesInvoiceItem : BaseInvoiceItem
    {
        public long SalesInvoiceId { get; set; }
        public decimal SalePrice { get; set; } // سعر البيع للمستهلك

        // حقول إضافية للعرض في الشاشة دون حفظها كأعمدة معقدة
        public string? ProductName { get; set; }
        public string? Barcode { get; set; }
    }
}
