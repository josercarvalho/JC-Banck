
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using BankMore.Infra.Database;
using Dapper;

namespace BankMore.Infra.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public MovimentoRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task Add(Movimento movimento)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync("INSERT INTO Movimento (Id, IdContaCorrente, DataMovimento, TipoMovimento, Valor) VALUES (@Id, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)", movimento);
            }
        }
    }
}
