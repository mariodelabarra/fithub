using Dapper;
using FitHub.Platform.Common.Domain;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace FitHub.Platform.Common.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Retrieves a single record by its <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all records of the specified type
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Inserts a new <paramref name="entity"/> into the table
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns>The number of rows affected</returns>
        Task<int> InsertAsync(TEntity entity);

        /// <summary>
        /// Updates an existing <paramref name="entity"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The number of rows affected</returns>
        Task<int> UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes a record by <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The number of rows affected</returns>
        Task<int> DeleteAsync(int id);
    }

    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IConfiguration _configuration;
        private MySqlConnection Connection => new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using var connection = Connection;
            connection.Open();

            var query = $"SELECT * FROM {typeof(TEntity).Name}s"; // Table name should be pluralized, if you follow that convention
            return await connection.QueryAsync<TEntity>(query);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            using var connection = Connection;
            connection.Open();

            var query = $"SELECT * FROM {typeof(TEntity).Name}s WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<TEntity>(query, new { Id = id });
        }

        public async Task<int> InsertAsync(TEntity entity)
        {
            using var connection = Connection;
            connection.Open();

            var query = $@"INSERT INTO {typeof(TEntity).Name}s ({string.Join(",", GetProperties(entity).Select(prop => $"{prop}"))})
                            VALUES ({string.Join(",", GetProperties(entity).Select(prop => $"@{prop}"))})";

            return await connection.ExecuteAsync(query, entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = Connection;
            connection.Open();

            var query = $"DELETE FROM {typeof(TEntity).Name}s WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            using var connection = Connection;
            connection.Open();

            var query = $@"UPDATE {typeof(TEntity).Name}s SET {string.Join(", ", GetProperties(entity).Select(prop => $"{prop} = @{prop}"))}
                        WHERE Id = @Id";

            return await connection.ExecuteAsync(query, entity);
        }

        // Helper method to get the property names of the entity
        private static IEnumerable<string> GetProperties(TEntity entity)
        {
            return typeof(TEntity).GetProperties()
                .Where(p =>
                {
                    var value = p.GetValue(entity);
                    var defaultValue = p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null;
                    return value is not null && !value.Equals(defaultValue);
                })
                .Select(p => p.Name);
        }
    }
}
