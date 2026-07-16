using System.Collections.Generic;
using System.Threading.Tasks;
using UrPOS.Core.Entities;

namespace UrPOS.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByBarcodeAsync(string barcode);
        Task<IEnumerable<Product>> SearchByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<int> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> UpdateStockAsync(int productId, int quantityChange);
    }
}