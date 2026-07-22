using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;

namespace UrPOS.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ServiceResult<int>> CreateProductAsync(Product product)
        {
            // 1. Validation: التحقق من وجود الباركود مسبقاً لمنع التكرار
            var existingProduct = await _productRepository.GetByBarcodeAsync(product.Barcode);
            if(existingProduct != null)
            {
                return ServiceResult<int>.Failure($"الباركود ({product.Barcode}) مُسجل مسبقاً لمنتج آخر: {existingProduct.ProductName}");
            }

            // 2. Validation: التحقق من أن سعر البيع لا يقل عن سعر الكلفة
            if (product.SalePrice < product.CostPrice)
            {
                return ServiceResult<int>.Failure("سعر البيع لا يمكن أن يكون أقل من سعر الكلفة.");
            }

            var success = await _productRepository.UpdateAsync(product);
            return success ? ServiceResult<int>.Success(product.Id) : ServiceResult<int>.Failure("فشل في إنشاء المنتج.");
        }

        public async Task<ServiceResult> UpdateProductAsync(Product product)
        {
            if(product.SalePrice < product.CostPrice)
            {
                return ServiceResult.Failure("خطأ تجاري: سعر البيع لا يمكن أن يكون أقل من سعر الكلفة.");
            }

            var success = await _productRepository.UpdateAsync(product);
            return success ? ServiceResult.Success() : ServiceResult.Failure("فشل تحديث بيانات المنتج، ربما تم حذفه.");
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            var allProducts = await _productRepository.GetAllAsync();
            // تصفية المواد التي وصل مخزونها للحد الأدنى المحدد أو أقل
            return allProducts.Where(p => p.CurrentStock <= p.MinStockLevel);
        }
    }
}
