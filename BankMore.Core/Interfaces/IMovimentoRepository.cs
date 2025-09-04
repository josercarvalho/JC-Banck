
using BankMore.Core.Entities;
using System.Threading.Tasks;

namespace BankMore.Core.Interfaces
{
    public interface IMovimentoRepository
    {
        Task Add(Movimento movimento);
    }
}
