using System.Threading.Tasks;
using Moq;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Services;
using Xunit;

namespace UrPOS.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateProduct_ShouldFail_WhenSalePriceIsLessThanCostPrice()
        {
            // Arrange (تجهيز البيئة والـ Mocks)
            var mockRepo = new Mock<IProductRepository>();
            var productService = new ProductService(mockRepo.Object);

            var invalidProduct = new Product
            {
                Barcode = "123456",
                ProductName = "منتج تجريبي",
                CostPrice = 1000,
                SalePrice = 500 // خطأ: سعر البيع أقل من الكلفة!
            };

            // Act (تنفيذ العملية)
            var result = await productService.CreateProductAsync(invalidProduct);

            // Assert (التحقق من النتيجة)
            Assert.False(result.isSuccess);
            Assert.Contains("سعر البيع لا يمكن أن يكون أقل من سعر الكلفة", result.ErrorMessage);
        }
    }
}