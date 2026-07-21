using System;
using System.Threading.Tasks;
using UrPOS.Core.Interfaces;
using UrPOS.Core.Entities;

namespace UrPOS.Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository, IProductRepository productRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
        }

        public async Task<ServiceResult<long>> ProcessSalesInvoiceAsync(SalesInvoice invoice)
        {
            // 1. Validation: التأكد من وجود عناصر داخل الفاتورة
            if(invoice.Items == null || invoice.Items.Count == 0)
            {
                return ServiceResult<long>.Failure("لا يمكن حفظ فاتورة فارغة بدون عناصر.");
            }

            // 2. Validation: فحص توفر الكميات في المخزن لكل عنصر قبل بدء البيع
            foreach (var item in invoice.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if(product == null)
                {
                    return ServiceResult<long>.Failure($"المنتج برقم ({item.ProductId}) غير موجود في النظام.");
                }

                if(product.CurrentStock < item.Quantity)
                {
                    return ServiceResult<long>.Failure($"الكمية غير كافية للمنتج ({product.ProductName}). المتاح في المخزن: {product.CurrentStock}، المطلوب: {item.Quantity}");
                }
            }

            // 3. ربط المستخدم الحالي التلقائي من كائن الجلسة الحية (UserSession)
            if(UserSession.Instance.IsLoggedIn && UserSession.Instance.UserId.HasValue)
            {
                invoice.UserId = UserSession.Instance.UserId.Value;
            }

            // 4. تنفيذ العملية في الباك آيند
            var invoiceId = await _invoiceRepository.SaveSalesInvoiceAsync(invoice);
            return ServiceResult<long>.Success(invoiceId);

        }

        public async Task<ServiceResult<long>> ProcessPurchaseInvoiceAsync(PurchaseInvoice invoice)
        {
            // 1. التأكد من وجود عناصر داخل الفاتورة
            if (invoice.Items == null || invoice.Items.Count == 0)
            {
                return ServiceResult<long>.Failure("لا يمكن حفظ فاتورة شراء فارغة .");
            }

            // 2. ربط المستخدم الحالي التلقائي من كائن الجلسة الحية (UserSession)
            if (UserSession.Instance.IsLoggedIn && UserSession.Instance.UserId.HasValue)
            {
                invoice.UserId = UserSession.Instance.UserId.Value;
            }

            // 3. تنفيذ العملية في الباك آيند
            var invoiceId = await _invoiceRepository.SavePurchaseInvoiceAsync(invoice);
            return ServiceResult <long>.Success(invoiceId);
        }
    }
}
