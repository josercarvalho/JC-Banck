
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using BankMore.Infra.Database;
using Dapper;

namespace BankMore.Infra.Repositories
{
    public class TransferenciaRepository : ITransferenciaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public TransferenciaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task Add(Transferencia transferencia)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync("INSERT INTO Transferencia (Id, IdContaCorrenteOrigem, IdContaCorrenteDestino, DataTransferencia, Valor) VALUES (@Id, @IdContaCorrenteOrigem, @IdContaCorrenteDestino, @DataTransferencia, @Valor)", transferencia);
            }
        }
    }
}
