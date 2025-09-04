
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace BankMore.Infra.Repositories
{
    public class TransferenciaRepository : ITransferenciaRepository
    {
        private readonly string _connectionString;

        public TransferenciaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Add(Transferencia transferencia)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("INSERT INTO Transferencia (Id, IdContaCorrenteOrigem, IdContaCorrenteDestino, DataTransferencia, Valor) VALUES (@Id, @IdContaCorrenteOrigem, @IdContaCorrenteDestino, @DataTransferencia, @Valor)", transferencia);
            }
        }
    }
}
