using BankMore.Core.Commands;
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BankMore.Core.Handlers
{
    public class MovimentacaoContaCorrenteCommandHandler : IRequestHandler<MovimentacaoContaCorrenteCommand>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentacaoContaCorrenteCommandHandler(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository, IIdempotenciaRepository idempotenciaRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<Unit> Handle(MovimentacaoContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            var idempotencia = await _idempotenciaRepository.GetById(request.IdRequisicao);
            if (idempotencia != null)
            {
                return Unit.Value;
            }

            if (!request.IdContaCorrente.HasValue)
            {
                throw new System.Exception("Id da conta corrente não informado.");
            }

            var contaCorrente = await _contaCorrenteRepository.GetById(request.IdContaCorrente.Value);

            if (contaCorrente == null)
            {
                throw new System.Exception("Conta corrente não encontrada.");
            }

            if (!contaCorrente.Ativo)
            {
                throw new System.Exception("Conta corrente inativa.");
            }

            if (request.TipoMovimento == "C")
            {
                contaCorrente.Creditar(request.Valor);
            }
            else if (request.TipoMovimento == "D")
            {
                contaCorrente.Debitar(request.Valor);
            }
            else
            {
                throw new System.Exception("Tipo de movimento inválido.");
            }

            var movimento = new Movimento(contaCorrente.Id, request.TipoMovimento, request.Valor);

            await _contaCorrenteRepository.Update(contaCorrente);
            await _movimentoRepository.Add(movimento);
            await _idempotenciaRepository.Add(new Idempotencia(request.IdRequisicao, "", ""));

            return Unit.Value;
        }
    }
}