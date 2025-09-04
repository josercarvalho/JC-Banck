
using BankMore.Core.Interfaces;
using BankMore.Core.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BankMore.Core.Handlers
{
    public class GetSaldoQueryHandler : IRequestHandler<GetSaldoQuery, decimal>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public GetSaldoQueryHandler(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<decimal> Handle(GetSaldoQuery request, CancellationToken cancellationToken)
        {
            var contaCorrente = await _contaCorrenteRepository.GetById(request.IdContaCorrente);

            if (contaCorrente == null)
            {
                throw new System.Exception("Conta corrente não encontrada.");
            }

            if (!contaCorrente.Ativo)
            {
                throw new System.Exception("Conta corrente inativa.");
            }

            return contaCorrente.Saldo;
        }
    }
}
