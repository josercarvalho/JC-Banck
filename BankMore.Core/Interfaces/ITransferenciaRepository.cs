
using BankMore.Core.Entities;
using System.Threading.Tasks;

namespace BankMore.Core.Interfaces
{
    public interface ITransferenciaRepository
    {
        Task Add(Transferencia transferencia);
    }
}
