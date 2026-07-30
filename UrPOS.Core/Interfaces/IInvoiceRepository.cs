using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrPOS.Core.Entities;

namespace UrPOS.Core.Interfaces
{
    public interface IInvoiceRepository
    {
        // عمليات المبيعات
        Task<long> SaveSalesInvoiceAsync(SalesInvoice invoice);
        Task<SalesInvoice?> GetSalesInvoiceByIdAsync(long invoiceId);
        Task<IEnumerable<SalesInvoiceListItem>> SearchSalesInvoicesAsync(
            string? invoiceNumber,
            DateTime? fromDate,
            DateTime? toDate);

        // عمليات المشتريات (سيناريو المندوب)
        Task<long> SavePurchaseInvoiceAsync(PurchaseInvoice invoice);
        Task<PurchaseInvoice?> GetPurchaseInvoiceByIdAsync(long invoiceId);
        Task<IEnumerable<PurchaseInvoice>> GetPurchaseInvoicesBySupplierIdAsync(int supplierId);
    }
}
