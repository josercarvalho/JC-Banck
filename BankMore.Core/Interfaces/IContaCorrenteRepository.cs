
using BankMore.Core.Entities;
using System;
using System.Threading.Tasks;

namespace BankMore.Core.Interfaces
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> GetById(Guid id);
        Task<ContaCorrente> GetByNumero(int numero);
        Task Add(ContaCorrente contaCorrente);
        Task Update(ContaCorrente contaCorrente);
    }
}
