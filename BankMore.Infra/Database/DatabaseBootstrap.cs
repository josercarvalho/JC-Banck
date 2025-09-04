
using Dapper;
using Npgsql;

namespace BankMore.Infra.Database
{
    public class DatabaseBootstrap
    {
        private readonly string _connectionString;

        public DatabaseBootstrap(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute("CREATE TABLE IF NOT EXISTS ContaCorrente (Id UUID PRIMARY KEY, Numero INT, Nome VARCHAR(100), Ativo BOOLEAN, Senha TEXT, Salt TEXT, Saldo DECIMAL(18, 2))");
                connection.Execute("CREATE TABLE IF NOT EXISTS Movimento (Id UUID PRIMARY KEY, IdContaCorrente UUID, DataMovimento TIMESTAMP, TipoMovimento VARCHAR(1), Valor DECIMAL(18, 2))");
                connection.Execute("CREATE TABLE IF NOT EXISTS Transferencia (Id UUID PRIMARY KEY, IdContaCorrenteOrigem UUID, IdContaCorrenteDestino UUID, DataTransferencia TIMESTAMP, Valor DECIMAL(18, 2))");
                connection.Execute("CREATE TABLE IF NOT EXISTS Idempotencia (ChaveIdempotencia UUID PRIMARY KEY, Requisicao TEXT, Resultado TEXT)");
            }
        }
    }
}
