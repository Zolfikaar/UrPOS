using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using UrPOS.Core.Entities;

namespace UrPOS.Infrastructure.Data
{
    public class DbConnectionFactory
    {
        private readonly AppConfigurations _configs;

        static DbConnectionFactory()
        {
            // يجب تفعيله قبل أي استعلام Dapper؛ وإلا يُخزَّن TypeMap بدون مطابقة الـ underscore
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public DbConnectionFactory(AppConfigurations configs)
        {
            _configs = configs;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_configs.GetConnectionString());
            await connection.OpenAsync();
            return connection;
        }
    }
}