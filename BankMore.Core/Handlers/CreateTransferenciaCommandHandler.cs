
using BankMore.Core.Commands;
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BankMore.Core.Handlers
{
    public class CreateTransferenciaCommandHandler : IRequestHandler<CreateTransferenciaCommand>
    {
        private readonly ITransferenciaRepository _transferenciaRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly IMediator _mediator;

        public CreateTransferenciaCommandHandler(ITransferenciaRepository transferenciaRepository, IIdempotenciaRepository idempotenciaRepository, IMediator mediator)
        {
            _transferenciaRepository = transferenciaRepository;
            _idempotenciaRepository = idempotenciaRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateTransferenciaCommand request, CancellationToken cancellationToken)
        {
            var idempotencia = await _idempotenciaRepository.GetById(request.IdRequisicao);
            if (idempotencia != null)
            {
                return Unit.Value;
            }

            var transferencia = new Transferencia(request.IdContaCorrenteOrigem, request.IdContaCorrenteDestino, request.Valor);

            await _mediator.Send(new MovimentacaoContaCorrenteCommand
            {
                IdRequisicao = request.IdRequisicao,
                IdContaCorrente = request.IdContaCorrenteOrigem,
                Valor = request.Valor,
                TipoMovimento = "D"
            });

            try
            {
                await _mediator.Send(new MovimentacaoContaCorrenteCommand
                {
                    IdRequisicao = request.IdRequisicao,
                    IdContaCorrente = request.IdContaCorrenteDestino,
                    Valor = request.Valor,
                    TipoMovimento = "C"
                });
            }
            catch (System.Exception)
            {
                await _mediator.Send(new MovimentacaoContaCorrenteCommand
                {
                    IdRequisicao = request.IdRequisicao,
                    IdContaCorrente = request.IdContaCorrenteOrigem,
                    Valor = request.Valor,
                    TipoMovimento = "C"
                });
                throw;
            }

            await _transferenciaRepository.Add(transferencia);
            await _idempotenciaRepository.Add(new Idempotencia(request.IdRequisicao, "", ""));

            return Unit.Value;
        }
    }
}
