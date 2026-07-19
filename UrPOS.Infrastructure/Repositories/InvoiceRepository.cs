using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Data;


namespace UrPOS.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public InvoiceRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        // =================================================================
        // 1. موديول المبيعات (Sales Module)
        // =================================================================

        public async Task<long> SaveSalesInvoiceAsync(SalesInvoice invoice)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                const string insertInvoiceQuery = @"
                INSERT INTO seles_invoices (invoice_number, user_id, total_amount, discount, net_amount, payment_type)
                VALUES (@InvoiceNumber, @UserId, @TotalAmount, @Discount, @NetAmount, @PaymentType);";

                var invoiceId = await connection.ExecuteScalarAsync<long>(insertInvoiceQuery, invoice, transaction);

                const string insertItemQuery = @"
                INSERT INTO sales_invoice_items (invoice_id, product_id, quantity, unit_price, total_price)
                VALUES(@SalesInvoiceId, @ProductId, @Quantity, @SalePrice, @TotalPrice);";

                const string insertMovementQuery = @"
                INSERT INTO inventory_movements (product_id, movement_type, quantity, reference_id, notes)
                VALUES (@ProductId, 'OUT', @Quantity, @ReferenceId, @Notes);";

                const string updateStockSql = @"
                    UPDATE products SET current_stock = current_stock - @Quantity WHERE id = @ProductId;";

                foreach (var item in invoice.Items)
                {
                    item.SalesInvoiceId = invoiceId;
                    await connection.ExecuteAsync(insertItemQuery, item, transaction);

                    // تسجيل حركة المخزن بالسالب للسحب من المخزون او البيع
                    await connection.ExecuteAsync(insertMovementQuery, new
                    {
                        item.ProductId,
                        item.Quantity,
                        ReferenceId = invoiceId,
                        Notes = $"Sales Invoice #{invoice.InvoiceNumber}"
                    }, transaction);

                    await connection.ExecuteAsync(updateStockSql, new { item.ProductId, item.Quantity }, transaction);
                }

                transaction.Commit();

                return invoiceId;

            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<SalesInvoice?> GetSalesInvoiceByIdAsync(long id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string invoiceSql = "SELECT * FROM sales_invoices WHERE id = @Id;";
            var invoice = await connection.QueryFirstOrDefaultAsync<SalesInvoice>(invoiceSql, new { Id = id });

            if (invoice != null)
            {
                const string itemsSql = @"
                    SELECT i.*, p.product_name as ProductName, p.barcode as Barcode, i.unit_price as SalePrice 
                    FROM sales_invoice_items i
                    JOIN products p ON i.product_id = p.id
                    WHERE i.invoice_id = @InvoiceId;";
                var items = await connection.QueryAsync<SalesInvoiceItem>(itemsSql, new { InvoiceId = id });
                invoice.Items = items.AsList();
            }
            return invoice;
        }


        // =================================================================
        // 2. موديول المشتريات - المندوبين (Purchase Module)
        // =================================================================

        public async Task<long> SavePurchaseInvoiceAsync(PurchaseInvoice invoice)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                const string insertInvoiceSql = @"
                    INSERT INTO purchase_invoices (invoice_number, supplier_id, user_id, total_amount, tax, net_amount, payment_type)
                    VALUES (@InvoiceNumber, @SupplierId, @UserId, @TotalAmount, @Tax, @NetAmount, @PaymentType)
                    RETURNING id;";

                var invoiceId = await connection.ExecuteScalarAsync<long>(insertInvoiceSql, invoice, transaction);

                if (invoice.PaymentType.ToUpper() == "CREDIT")
                {
                    const string updateSupplierSql = "UPDATE suppliers SET balance = balance - @NetAmount WHERE id = @SupplierId;";
                    await connection.ExecuteAsync(updateSupplierSql, new { invoice.SupplierId, invoice.NetAmount }, transaction);
                }

                const string insertItemSql = @"
                    INSERT INTO purchase_invoice_items (purchase_invoice_id, product_id, quantity, cost_price, total_price)
                    VALUES (@PurchaseInvoiceId, @ProductId, @Quantity, @CostPrice, @TotalPrice);";

                const string insertMovementSql = @"
                    INSERT INTO inventory_movements (product_id, movement_type, quantity, reference_id, notes)
                    VALUES (@ProductId, 'IN', @Quantity, @ReferenceId, @Notes);";

                const string updateProductSql = @"
                    UPDATE products SET current_stock = current_stock + @Quantity, cost_price = @CostPrice WHERE id = @ProductId;";

                foreach (var item in invoice.Items)
                {
                    item.PurchaseInvoiceId = invoiceId;
                    await connection.ExecuteAsync(insertItemSql, item, transaction);

                    await connection.ExecuteAsync(insertMovementSql, new
                    {
                        item.ProductId,
                        item.Quantity,
                        ReferenceId = invoiceId,
                        Notes = $"Purchase Invoice #{invoice.InvoiceNumber}"
                    }, transaction);

                    await connection.ExecuteAsync(updateProductSql, new { item.ProductId, item.Quantity, item.CostPrice }, transaction);
                }

                transaction.Commit();
                return invoiceId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public async Task<PurchaseInvoice?> GetPurchaseInvoiceByIdAsync(long id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string invoiceSql = "SELECT * FROM purchase_invoices WHERE id = @Id;";
            var invoice = await connection.QueryFirstOrDefaultAsync<PurchaseInvoice>(invoiceSql, new { Id = id });

            if (invoice != null)
            {
                const string itemsSql = "SELECT * FROM purchase_invoice_items WHERE purchase_invoice_id = @InvoiceId;";
                var items = await connection.QueryAsync<PurchaseInvoiceItem>(itemsSql, new { InvoiceId = id });
                invoice.Items = items.AsList();
            }
            return invoice;
        }

        public async Task<IEnumerable<PurchaseInvoice>> GetPurchaseInvoicesBySupplierIdAsync(int supplierId)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = "SELECT * FROM purchase_invoices WHERE supplier_id = @SupplierId ORDER BY id DESC;";
            return await connection.QueryAsync<PurchaseInvoice>(sql, new { SupplierId = supplierId });
        }

    }
}
