using Dapper;
using LU2Raf.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace LU2Raf.Repositories
{
    public class Environment2DRepository : IEnvironment2DRepository
    {
        private readonly string _sqlConnectionString;

        public Environment2DRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        public async Task<IEnumerable<Environment2D>> GetAllAsync()
        {
            using var connection = new SqlConnection(_sqlConnectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<Environment2D>("SELECT Id, Name, MinLength, MaxLength FROM Environment2D");
        }
        public async Task<IEnumerable<Environment2D>> GetByOwnerUserIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_sqlConnectionString);
            await connection.OpenAsync();
            string query = "SELECT * FROM Environment2D WHERE OwnerUserId = @OwnerUserId";
            return await connection.QueryAsync<Environment2D>(query, new { OwnerUserId = id });
        }

        public async Task AddAsync(Environment2D environment)
        {
            environment.Id = Guid.NewGuid();
            using var connection = new SqlConnection(_sqlConnectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO Environment2D (Id, Name, MinLength, MaxLength, OwnerUserId) VALUES (@Id, @Name, @MinLength, @MaxLength, @OwnerUserId)";

            await connection.ExecuteAsync(query, new
            {
                Id = environment.Id,
                Name = environment.Name,
                MinLength = environment.MinLength,
                MaxLength = environment.MaxLength,
                OwnerUserId = environment.OwnerUserId 
            });
        }

        public async Task DeleteAsync(string environmentName)
        {
            if (string.IsNullOrEmpty(environmentName))
            {
                throw new ArgumentException("De environment naam mag niet leeg zijn.", nameof(environmentName));
            }

            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                await connection.OpenAsync();

                // Haal het EnvironmentId op
                string getIdQuery = "SELECT Id FROM Environment2D WHERE Name = @Name";
                var environmentId = await connection.ExecuteScalarAsync<Guid>(getIdQuery, new { Name = environmentName });

                if (environmentId == Guid.Empty)
                {
                    throw new KeyNotFoundException($"Geen environment gevonden met de naam: {environmentName}");
                }

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Verwijder alle objecten die aan dit environment gekoppeld zijn
                        string deleteObjectsQuery = "DELETE FROM Object2D WHERE EnvironmentId = @EnvironmentId";
                        await connection.ExecuteAsync(deleteObjectsQuery, new { EnvironmentId = environmentId }, transaction);

                        // Verwijder het environment zelf
                        string deleteEnvironmentQuery = "DELETE FROM Environment2D WHERE Id = @EnvironmentId";
                        await connection.ExecuteAsync(deleteEnvironmentQuery, new { EnvironmentId = environmentId }, transaction);

                        // Bevestig de transacties
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}
