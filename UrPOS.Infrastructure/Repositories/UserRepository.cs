using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Data;

namespace UrPOS.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public UserRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        // أعمدة PostgreSQL snake_case تُعاد بأسماء صريحة (PascalCase) لضمان مطابقة Dapper مع خصائص User
        private const string UserSelectSql = @"
            SELECT
                u.id            AS Id,
                u.username      AS Username,
                u.password_hash AS PasswordHash,
                u.full_name     AS FullName,
                u.is_active     AS IsActive,
                u.created_at    AS CreatedAt,
                r.role_name     AS RoleName
            FROM users u
            LEFT JOIN user_roles ur ON u.id = ur.user_id
            LEFT JOIN roles r ON ur.role_id = r.id";

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var sql = UserSelectSql + " WHERE u.id = @Id;";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var sql = UserSelectSql + " WHERE LOWER(u.username) = LOWER(@Username);";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var sql = UserSelectSql + " ORDER BY u.id DESC;";
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<int> AddAsync(User user, int roleId)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            // نفتح Transaction لأننا سنقوم بالإدخال في جدولين (المستخدمين ثم جدول ربط الأدوار) لضمان الـ Integrity
            using var transaction = connection.BeginTransaction();
            try
            {
                const string insertUserSql = @"
                    INSERT INTO users (username, password_hash, full_name, is_active)
                    VALUES (@Username, @PasswordHash, @FullName, @IsActive)
                    RETURNING id;";

                var userId = await connection.ExecuteScalarAsync<int>(insertUserSql, user, transaction);

                const string insertRoleSql = @"
                    INSERT INTO user_roles (user_id, role_id)
                    VALUES (@UserId, @RoleId);";

                await connection.ExecuteAsync(insertRoleSql, new { UserId = userId, RoleId = roleId }, transaction);

                transaction.Commit();
                return userId;
            }
            catch
            {
                transaction.Rollback();
                throw; // إعادة رمي الخطأ للتعامل معه في الطبقات الأعلى
            }
        }

        public async Task<bool> UpdateAsync(User user)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = @"
                UPDATE users 
                SET username = @Username, 
                    full_name = @FullName, 
                    is_active = @IsActive
                WHERE id = @Id;";

            var rowsAffected = await connection.ExecuteAsync(sql, user);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateStatusAsync(int userId, bool isActive)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            const string sql = "UPDATE users SET is_active = @IsActive WHERE id = @Id;";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = userId, IsActive = isActive });
            return rowsAffected > 0;
        }
    }
}