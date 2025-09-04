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
                return (await connection.QueryFirstOrDefaultAsync<ContaCorrente>("SELECT * FROM ContaCorrente WHERE NumeroConta = @numero", new { numero }))!;
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
                await connection.ExecuteAsync("INSERT INTO ContaCorrente (Id, NumeroConta, Nome, Cpf, Ativo, Senha, Salt, Saldo) VALUES (@Id, @NumeroConta, @Nome, @Cpf, @Ativo, @Senha, @Salt, @Saldo)", contaCorrente);
            }
        }

        public async Task Update(ContaCorrente contaCorrente)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("UPDATE ContaCorrente SET Nome = @Nome, Ativo = @Ativo, Senha = @Senha, Salt = @Salt, Saldo = @Saldo WHERE Id = @Id", contaCorrente);
            }
        }
    }
}