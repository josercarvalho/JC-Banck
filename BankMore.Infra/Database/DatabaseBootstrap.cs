
using Dapper;

namespace BankMore.Infra.Database
{
    public class DatabaseBootstrap
    {
        private readonly DbConnectionFactory _connectionFactory;

        public DatabaseBootstrap(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Setup()
        {
            const int maxRetries = 5;
            const int delayMilliseconds = 2000; // 2 seconds

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    using (var connection = _connectionFactory.CreateConnection())
                    {
                        connection.Open();

                        connection.Execute("CREATE TABLE IF NOT EXISTS ContaCorrente (Id UUID PRIMARY KEY, NumeroConta VARCHAR(20) NOT NULL UNIQUE, Nome VARCHAR(100), CPF VARCHAR(14) NOT NULL UNIQUE, Saldo DECIMAL(18, 2) NOT NULL, Ativa BOOLEAN NOT NULL, Senha VARCHAR(100) NOT NULL, Salt VARCHAR(100) NOT NULL, DataCriacao TIMESTAMP NOT NULL, DataAtualizacao TIMESTAMP NOT NULL)");
                        connection.Execute("CREATE TABLE IF NOT EXISTS Movimento (Id UUID PRIMARY KEY, IdContaCorrente UUID, DataMovimento TIMESTAMP, TipoMovimento VARCHAR(1), Valor DECIMAL(18, 2))");
                        connection.Execute("CREATE TABLE IF NOT EXISTS Transferencia (Id UUID PRIMARY KEY, IdContaCorrenteOrigem UUID, IdContaCorrenteDestino UUID, DataTransferencia TIMESTAMP, Valor DECIMAL(18, 2))");
                        connection.Execute("CREATE TABLE IF NOT EXISTS Idempotencia (ChaveIdempotencia UUID PRIMARY KEY, Requisicao TEXT, Resultado TEXT)");
                        
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {i + 1} of {maxRetries} failed to connect to database: {ex.Message}");
                    if (i < maxRetries - 1)
                    {
                        Thread.Sleep(delayMilliseconds);
                    }
                    else
                    {
                        throw; 
                    }
                }
            }
        }
    }
}
