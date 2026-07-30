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

        [Fact]
        public async Task CreateProduct_ShouldFail_WhenBarcodeAlreadyExists()
        {
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetByBarcodeAsync("DUP-001"))
                .ReturnsAsync(new Product { Id = 9, Barcode = "DUP-001", ProductName = "منتج قديم" });

            var productService = new ProductService(mockRepo.Object);
            var result = await productService.CreateProductAsync(new Product
            {
                Barcode = "DUP-001",
                ProductName = "منتج جديد",
                CostPrice = 100,
                SalePrice = 150
            });

            Assert.False(result.isSuccess);
            Assert.Contains("مُسجل مسبقاً", result.ErrorMessage);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}