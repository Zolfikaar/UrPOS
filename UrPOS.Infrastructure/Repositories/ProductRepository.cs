using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Data;

namespace UrPOS.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public ProductRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = "SELECT * FROM products WHERE id = @Id;";
            return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<Product?> GetByBarcodeAsync(string barcode)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = "SELECT * FROM products WHERE barcode = @Barcode;";
            return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Barcode = barcode });
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = "SELECT * FROM products ORDER BY id DESC;";
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<int> AddAsync(Product product)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                INSERT INTO products (barcode, product_name, cost_price, sale_price, min_stock_level, current_stock, unit_of_measure, custom_attributes)
                VALUES (@Barcode, @ProductName, @CostPrice, @SalePrice, @MinStockLevel, @CurrentStock, @UnitOfMeasure, @CustomAttributes::jsonb)
                RETURNING id;";

            return await connection.ExecuteScalarAsync<int>(sql, product);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                UPDATE products 
                SET barcode = @Barcode, 
                    product_name = @ProductName, 
                    cost_price = @CostPrice, 
                    sale_price = @SalePrice, 
                    min_stock_level = @MinStockLevel,
                    unit_of_measure = @UnitOfMeasure,
                    custom_attributes = @CustomAttributes::jsonb
                WHERE id = @Id;";

            var rowsAffected = await connection.ExecuteAsync(sql, product);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantityChange)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            // تحديث تراكمي آمن لمنع الـ Race Conditions في العمليات المتزامنة
            const string sql = @"
                UPDATE products 
                SET current_stock = current_stock + @QuantityChange 
                WHERE id = @Id;";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = productId, QuantityChange = quantityChange });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = "SELECT * FROM products WHERE product_name ILIKE @Name ORDER BY id DESC;";
            return await connection.QueryAsync<Product>(sql, new { Name = $"%{name}%" });
        }
    }
}