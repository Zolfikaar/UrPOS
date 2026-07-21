using System.Collections.Generic;
using System.Threading.Tasks;
using UrPOS.Core.Entities;

namespace UrPOS.Core.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<int>> CreateProductAsync(Product product);
        Task<ServiceResult> UpdateProductAsync(Product product);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(); // جلب المواد التي وصلت للحد الأدنى من المخزون
    }
}
