using FluentValidation;
using System;

namespace BankMore.Core.Entities.Validators
{
    public class IdempotenciaValidator : AbstractValidator<Idempotencia>
    {
        public IdempotenciaValidator()
        {
            RuleFor(i => i.ChaveIdempotencia)
                .NotEqual(Guid.Empty).WithMessage("A chave de idempotência não pode ser vazia.");

            RuleFor(i => i.Requisicao)
                .NotEmpty().WithMessage("A requisição não pode ser vazia.");

            RuleFor(i => i.Resultado)
                .NotEmpty().WithMessage("O resultado não pode ser vazio.");
        }
    }
}
