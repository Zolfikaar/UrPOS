using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Data;

namespace UrPOS.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public SupplierRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> AddAsync(Supplier supplier)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                INSERT INTO suppliers (supplier_name, phone, balance)
                VALUES (@SupplierName, @Phone, @Balance)
                RETURNING id;";
            return await connection.ExecuteScalarAsync<int>(sql, supplier);
        }

        public async Task<bool> UpdateAsync(Supplier supplier)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                UPDATE suppliers
                SET supplier_name = @SupplierName,
                    phone = @Phone,
                    balance = @Balance
                WHERE id = @Id;";
            var rowsAffected = await connection.ExecuteAsync(sql, supplier);
            return rowsAffected > 0;
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = "SELECT id, supplier_name AS SupplierName, phone, balance FROM suppliers WHERE id = @Id;";
            return await connection.QueryFirstOrDefaultAsync<Supplier>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = "SELECT id, supplier_name AS SupplierName, phone, balance FROM suppliers ORDER BY id DESC;";
            return await connection.QueryAsync<Supplier>(sql);
        }

        // === العمليات المالية المضافة ===

        public async Task<bool> UpdateBalanceAsync(int supplierId, decimal amountDelta)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                UPDATE suppliers 
                SET balance = balance + @AmountDelta 
                WHERE id = @SupplierId;";
            var rowsAffected = await connection.ExecuteAsync(sql, new { SupplierId = supplierId, AmountDelta = amountDelta });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersWithBalanceAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                SELECT id, supplier_name AS SupplierName, phone, balance 
                FROM suppliers 
                WHERE balance <> 0 
                ORDER BY balance ASC;"; // ترتيبهم بحسب حجم الدين
            return await connection.QueryAsync<Supplier>(sql);
        }

        public async Task<IEnumerable<Supplier>> SearchAsync(string searchTerm)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                SELECT id, supplier_name AS SupplierName, phone, balance 
                FROM suppliers 
                WHERE LOWER(supplier_name) LIKE LOWER(@Search) OR phone LIKE @Search
                ORDER BY supplier_name ASC;";
            return await connection.QueryAsync<Supplier>(sql, new { Search = $"%{searchTerm}%" });
        }
    }
}