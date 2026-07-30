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
            if (string.IsNullOrWhiteSpace(product.Barcode))
            {
                return ServiceResult<int>.Failure("الباركود مطلوب.");
            }

            // 1. Validation: التحقق من وجود الباركود مسبقاً لمنع التكرار
            var existingProduct = await _productRepository.GetByBarcodeAsync(product.Barcode.Trim());
            if (existingProduct != null)
            {
                return ServiceResult<int>.Failure($"الباركود ({product.Barcode}) مُسجل مسبقاً لمنتج آخر: {existingProduct.ProductName}");
            }

            // 2. Validation: التحقق من أن سعر البيع لا يقل عن سعر الكلفة
            if (product.SalePrice < product.CostPrice)
            {
                return ServiceResult<int>.Failure("سعر البيع لا يمكن أن يكون أقل من سعر الكلفة.");
            }

            if (string.IsNullOrWhiteSpace(product.UnitOfMeasure))
            {
                product.UnitOfMeasure = "قطعة";
            }

            product.Barcode = product.Barcode.Trim();
            var newId = await _productRepository.AddAsync(product);
            if (newId <= 0)
            {
                return ServiceResult<int>.Failure("فشل في إنشاء المنتج.");
            }

            product.Id = newId;
            return ServiceResult<int>.Success(newId);
        }

        public async Task<ServiceResult> UpdateProductAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Barcode))
            {
                return ServiceResult.Failure("الباركود مطلوب.");
            }

            if (product.SalePrice < product.CostPrice)
            {
                return ServiceResult.Failure("خطأ تجاري: سعر البيع لا يمكن أن يكون أقل من سعر الكلفة.");
            }

            var existingByBarcode = await _productRepository.GetByBarcodeAsync(product.Barcode.Trim());
            if (existingByBarcode != null && existingByBarcode.Id != product.Id)
            {
                return ServiceResult.Failure($"الباركود ({product.Barcode}) مُسجل مسبقاً لمنتج آخر: {existingByBarcode.ProductName}");
            }

            if (string.IsNullOrWhiteSpace(product.UnitOfMeasure))
            {
                product.UnitOfMeasure = "قطعة";
            }

            product.Barcode = product.Barcode.Trim();
            var success = await _productRepository.UpdateAsync(product);
            return success ? ServiceResult.Success() : ServiceResult.Failure("فشل تحديث بيانات المنتج، ربما تم حذفه.");
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            var allProducts = await _productRepository.GetAllAsync();
            // تصفية المواد التي وصل مخزونها للحد الأدنى المحدد أو أقل
            return allProducts.Where(p => p.CurrentStock <= p.MinStockLevel);
        }

        public Task<Product?> GetByBarcodeAsync(string barcode)
        {
            return _productRepository.GetByBarcodeAsync(barcode);
        }

        public Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            return _productRepository.SearchByNameAsync(name);
        }
    }
}
