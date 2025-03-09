using Dapper;
using LU2Raf.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System;

namespace LU2Raf.Repositories
{
    public class Object2DRepository : IObject2DRepository
    {
        private readonly string _sqlConnectionString;

        public Object2DRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        public async Task<IEnumerable<Object2D>> GetAllAsync()
        {
            using var connection = new SqlConnection(_sqlConnectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<Object2D>("SELECT Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer FROM Object2D");
        }

        public async Task<Object2D> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_sqlConnectionString);
            await connection.OpenAsync();
            string query = "SELECT* FROM Object2D WHERE Id = CAST(@Id AS UNIQUEIDENTIFIER)";
            return await connection.QuerySingleOrDefaultAsync<Object2D>(query, new { id });
        }
        public async Task AddAsync(Object2D obj)
        {
            obj.Id = Guid.NewGuid();
            using var connection = new SqlConnection(_sqlConnectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO Object2D (Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, EnvironmentId) " +
                           "VALUES (@Id, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @EnvironmentId)";

            await connection.ExecuteAsync(query, new
            {
                Id = obj.Id,
                PrefabId = obj.PrefabId,
                PositionX = obj.PositionX,
                PositionY = obj.PositionY,
                ScaleX = obj.ScaleX,
                ScaleY = obj.ScaleY,
                RotationZ = obj.RotationZ,
                SortingLayer = obj.SortingLayer,
                EnvironmentId = obj.EnvironmentId
            });
        }
    }
}
