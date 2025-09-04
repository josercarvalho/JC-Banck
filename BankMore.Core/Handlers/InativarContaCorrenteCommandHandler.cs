
using BankMore.Core.Commands;
using BankMore.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BankMore.Core.Handlers
{
    public class InativarContaCorrenteCommandHandler : IRequestHandler<InativarContaCorrenteCommand>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public InativarContaCorrenteCommandHandler(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<Unit> Handle(InativarContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            var contaCorrente = await _contaCorrenteRepository.GetById(request.Id);

            if (contaCorrente == null || contaCorrente.Senha != request.Senha)
            {
                throw new System.UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            contaCorrente.Inativar();
            await _contaCorrenteRepository.Update(contaCorrente);

            return Unit.Value;
        }
    }
}
