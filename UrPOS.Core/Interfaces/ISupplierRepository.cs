using System.Collections.Generic;
using System.Threading.Tasks;
using UrPOS.Core.Entities;

namespace UrPOS.Core.Interfaces
{
    public interface ISupplierRepository
    {
        // العمليات الأساسية
        Task<int> AddAsync(Supplier supplier);
        Task<bool> UpdateAsync(Supplier supplier);
        Task<Supplier?> GetByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllAsync();

        // العمليات المالية والتجميعية للموردين
        Task<bool> UpdateBalanceAsync(int supplierId, decimal amountDelta); // إضافة أو خصم مبلغ من الرصيد
        Task<IEnumerable<Supplier>> GetSuppliersWithBalanceAsync(); // جلب الموردين أصحاب الحسابات المعلقة (Balance != 0)
        Task<IEnumerable<Supplier>> SearchAsync(string searchTerm); // بحث باسم المورد أو رقم الهاتف
    }
}