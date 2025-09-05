using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using BankMore.Infra.Database;
using Dapper;

namespace BankMore.Infra.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public IdempotenciaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Idempotencia> GetById(Guid chaveIdempotencia)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return (await connection.QueryFirstOrDefaultAsync<Idempotencia>("SELECT * FROM Idempotencia WHERE ChaveIdempotencia = @chaveIdempotencia", new { chaveIdempotencia }))!;
            }
        }

        public async Task Add(Idempotencia idempotencia)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync("INSERT INTO Idempotencia (ChaveIdempotencia, Requisicao, Resultado) VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)", idempotencia);
            }
        }
    }
}