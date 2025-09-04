
using BankMore.Core.Entities;
using System;
using System.Threading.Tasks;

namespace BankMore.Core.Interfaces
{
    public interface IIdempotenciaRepository
    {
        Task<Idempotencia> GetById(Guid chaveIdempotencia);
        Task Add(Idempotencia idempotencia);
    }
}
