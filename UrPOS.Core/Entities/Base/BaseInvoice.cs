using System;

namespace UrPOS.Core.Entities.Base
{
    // الكلاس الأساسي المشترك لرأس أي فاتورة
    public abstract class BaseInvoice
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string PaymentType { get; set; } = "CASH"; // CASH, CREDIT
        public DateTime CreatedAt { get; set; }

    }

    // الكلاس المشترك لسطر أي فاتورة
    public abstract class BaseInvoiceItem
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
