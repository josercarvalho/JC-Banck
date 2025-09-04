using FluentValidation;
using System;

namespace BankMore.Core.Entities.Validators
{
    public class TransferenciaValidator : AbstractValidator<Transferencia>
    {
        public TransferenciaValidator()
        {
            RuleFor(t => t.IdContaCorrenteOrigem)
                .NotEqual(Guid.Empty).WithMessage("O Id da conta de origem não pode ser vazio.");

            RuleFor(t => t.IdContaCorrenteDestino)
                .NotEqual(Guid.Empty).WithMessage("O Id da conta de destino não pode ser vazio.");

            RuleFor(t => t.Valor)
                .GreaterThan(0).WithMessage("O valor da transferência deve ser maior que zero.");
        }
    }
}
