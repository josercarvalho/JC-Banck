using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using BankMore.Infra.Database;
using Dapper;

namespace BankMore.Infra.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public ContaCorrenteRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<ContaCorrente> GetById(Guid id)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return (await connection.QueryFirstOrDefaultAsync<ContaCorrente>("SELECT * FROM ContaCorrente WHERE Id = @id", new { Id = id }))!;
            }
        }

        public async Task<ContaCorrente> GetByNumero(string numero)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return (await connection.QueryFirstOrDefaultAsync<ContaCorrente>("SELECT * FROM ContaCorrente WHERE numeroconta = @numeroconta", new { NumeroConta = numero }))!;
            }
        }

        public async Task<ContaCorrente> GetByCpf(string cpf)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return (await connection.QueryFirstOrDefaultAsync<ContaCorrente>("SELECT * FROM ContaCorrente WHERE Cpf = @cpf", new { Cpf = cpf }))!;
            }
        }

        public async Task Add(ContaCorrente contaCorrente)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync("INSERT INTO ContaCorrente (Id, NumeroConta, Nome, Cpf, Ativa, Senha, Salt, Saldo, DataCriacao, DataAtualizacao) VALUES (@Id, @Numero, @Nome, @Cpf, @Ativo, @Senha, @Salt, @Saldo, @DataCriacao, @DataAtualizacao)", contaCorrente);
            }
        }

        public async Task Update(ContaCorrente contaCorrente)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync("UPDATE ContaCorrente SET Nome = @Nome, Ativa = @Ativo, Senha = @Senha, Salt = @Salt, Saldo = @Saldo WHERE Id = @Id", contaCorrente);
            }
        }

        public async Task<IEnumerable<ContaCorrente>> GetAll(int pageNumber, int pageSize)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var offset = (pageNumber - 1) * pageSize;
                return await connection.QueryAsync<ContaCorrente>($"SELECT * FROM ContaCorrente LIMIT {pageSize} OFFSET {offset}");
            }
        }
    }
}