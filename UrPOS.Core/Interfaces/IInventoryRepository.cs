using System.Collections.Generic;
using System.Threading.Tasks;
using UrPOS.Core.Entities;

namespace UrPOS.Core.Interfaces
{
    public interface IInventoryRepository
    {
        // حركات المخزن
        Task<long> AddMovementAsync(InventoryMovement movement);
        Task<IEnumerable<InventoryMovement>> GetProductMovementsAsync(int productId);

        // الموردين
        Task<int> AddSupplierAsync(Supplier supplier);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<bool> UpdateSupplierBalanceAsync(int supplierId, decimal amountChange);
    }
}