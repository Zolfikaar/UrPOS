using UrPOS.Core.Entities;
using System.Threading.Tasks;

namespace UrPOS.Core.Interfaces
{
    public interface IInvoiceService
    {
        Task<ServiceResult<long>> ProcessSalesInvoiceAsync(SalesInvoice invoice);
        Task<ServiceResult<long>> ProcessPurchaseInvoiceAsync(PurchaseInvoice invoice);
    }
}
