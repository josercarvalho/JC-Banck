
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace BankMore.Infra.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly string _connectionString;

        public MovimentoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Add(Movimento movimento)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.ExecuteAsync("INSERT INTO Movimento (Id, IdContaCorrente, DataMovimento, TipoMovimento, Valor) VALUES (@Id, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)", movimento);
            }
        }
    }
}
