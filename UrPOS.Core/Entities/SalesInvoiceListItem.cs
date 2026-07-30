using System;

namespace UrPOS.Core.Entities
{
    /// <summary>
    /// Lightweight sales invoice row for history grids (completed DB invoices + parked drafts).
    /// </summary>
    public class SalesInvoiceListItem
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CashierName { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Status { get; set; } = InvoiceStatus.Completed;
        public string PaymentType { get; set; } = "CASH";

        /// <summary>Index into <see cref="ParkedInvoiceSession"/> when Status is Parked; otherwise -1.</summary>
        public int ParkedDraftIndex { get; set; } = -1;
    }

    public static class InvoiceStatus
    {
        public const string Completed = "Completed";
        public const string Parked = "Parked";

        public static string ToArabic(string status) => status switch
        {
            Parked => "معلقة",
            _ => "مكتملة"
        };
    }
}
