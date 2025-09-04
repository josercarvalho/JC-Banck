using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Threading.Tasks;

namespace BankMore.Infra.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly string _connectionString;

        public IdempotenciaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Idempotencia> GetById(Guid chaveIdempotencia)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return (await connection.QueryFirstOrDefaultAsync<Idempotencia>("SELECT * FROM Idempotencia WHERE ChaveIdempotencia = @chaveIdempotencia", new { chaveIdempotencia }))!;
            }
        }

        public async Task Add(Idempotencia idempotencia)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.ExecuteAsync("INSERT INTO Idempotencia (ChaveIdempotencia, Requisicao, Resultado) VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)", idempotencia);
            }
        }
    }
}