
using Dapper;
using Microsoft.Data.Sqlite;

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
            var connectionStringParts = _connectionString.Split('=');
            if (connectionStringParts.Length > 1)
            {
                var dbPath = connectionStringParts[1].Trim();
                var dbDir = System.IO.Path.GetDirectoryName(dbPath);
                if (!string.IsNullOrEmpty(dbDir) && !System.IO.Directory.Exists(dbDir))
                {
                    System.IO.Directory.CreateDirectory(dbDir);
                }
            }

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                connection.Execute("CREATE TABLE IF NOT EXISTS ContaCorrente (Id TEXT PRIMARY KEY, Numero INTEGER, Nome TEXT, Ativo INTEGER, Senha TEXT, Salt TEXT, Saldo REAL)");
                connection.Execute("CREATE TABLE IF NOT EXISTS Movimento (Id TEXT PRIMARY KEY, IdContaCorrente TEXT, DataMovimento TEXT, TipoMovimento TEXT, Valor REAL)");
                connection.Execute("CREATE TABLE IF NOT EXISTS Transferencia (Id TEXT PRIMARY KEY, IdContaCorrenteOrigem TEXT, IdContaCorrenteDestino TEXT, DataTransferencia TEXT, Valor REAL)");
                connection.Execute("CREATE TABLE IF NOT EXISTS Idempotencia (ChaveIdempotencia TEXT PRIMARY KEY, Requisicao TEXT, Resultado TEXT)");
            }
        }
    }
}
