using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace UrPOS.Infrastructure.Data
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            // جلب نص الاتصال من ملف الإعدادات
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new System.ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}