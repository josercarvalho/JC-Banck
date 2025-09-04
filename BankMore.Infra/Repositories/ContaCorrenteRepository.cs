using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using Dapper;
using Npgsql;

using System;
using System.Threading.Tasks;

namespace BankMore.Infra.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly string _connectionString;

        public ContaCorrenteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ContaCorrente> GetById(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection.QueryFirstOrDefaultAsync<ContaCorrente>("SELECT * FROM ContaCorrente WHERE Id = @id", new { id }))!;
            }
        }

        public async Task<ContaCorrente> GetByNumero(int numero)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection.QueryFirstOrDefaultAsync<ContaCorrente>("SELECT * FROM ContaCorrente WHERE numeroconta = @numero", new { numero }))!;
            }
        }

        public async Task<ContaCorrente> GetByCpf(string cpf)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection.QueryFirstOrDefaultAsync<ContaCorrente>("SELECT * FROM ContaCorrente WHERE Cpf = @cpf", new { cpf }))!;
            }
        }

        public async Task Add(ContaCorrente contaCorrente)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("INSERT INTO ContaCorrente (Id, NumeroConta, Nome, Cpf, Ativa, Senha, Salt, Saldo) VALUES (@Id, @Numero, @Nome, @Cpf, @Ativa, @Senha, @Salt, @Saldo)", contaCorrente);
            }
        }

        public async Task Update(ContaCorrente contaCorrente)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("UPDATE ContaCorrente SET Nome = @Nome, Ativa = @Ativo, Senha = @Senha, Salt = @Salt, Saldo = @Saldo WHERE Id = @Id", contaCorrente);
            }
        }

        public async Task<IEnumerable<ContaCorrente>> GetAll(int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var offset = (pageNumber - 1) * pageSize;
                return await connection.QueryAsync<ContaCorrente>($"SELECT * FROM ContaCorrente LIMIT {pageSize} OFFSET {offset}");
            }
        }
    }
}