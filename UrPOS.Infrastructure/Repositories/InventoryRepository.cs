using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Data;

namespace UrPOS.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public InventoryRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<long> AddMovementAsync(InventoryMovement movement)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                // 1. تسجيل الحركة في جدول الحركات
                const string insertSql = @"
                    INSERT INTO inventory_movements (product_id, movement_type, quantity, reference_id, notes)
                    VALUES (@ProductId, @MovementType, @Quantity, @ReferenceId, @Notes)
                    RETURNING id;";

                var movementId = await connection.ExecuteScalarAsync<long>(insertSql, movement, transaction);

                // 2. تحديث المخزن الحالي في جدول المنتجات بشكل تراكمي آمن
                const string updateStockSql = @"
                    UPDATE products 
                    SET current_stock = current_stock + @Quantity 
                    WHERE id = @ProductId;";

                await connection.ExecuteAsync(updateStockSql, new { movement.ProductId, movement.Quantity }, transaction);

                transaction.Commit();
                return movementId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<InventoryMovement>> GetProductMovementsAsync(int productId)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                SELECT m.*, p.product_name as ProductName 
                FROM inventory_movements m
                JOIN products p ON m.product_id = p.id
                WHERE m.product_id = @ProductId
                ORDER BY m.id DESC;";
            return await connection.QueryAsync<InventoryMovement>(sql, new { ProductId = productId });
        }

        public async Task<int> AddSupplierAsync(Supplier supplier)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                INSERT INTO suppliers (supplier_name, phone, balance)
                VALUES (@SupplierName, @Phone, @Balance)
                RETURNING id;";
            return await connection.ExecuteScalarAsync<int>(sql, supplier);
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = "SELECT * FROM suppliers ORDER BY id DESC;";
            return await connection.QueryAsync<Supplier>(sql);
        }

        public async Task<bool> UpdateSupplierBalanceAsync(int supplierId, decimal amountChange)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                UPDATE suppliers 
                SET balance = balance + @AmountChange 
                WHERE id = @SupplierId;";
            var rows = await connection.ExecuteAsync(sql, new { SupplierId = supplierId, AmountChange = amountChange });
            return rows > 0;
        }
    }
}