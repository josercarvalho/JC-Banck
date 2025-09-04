using FluentValidation;
using System;

namespace BankMore.Core.Entities.Validators
{
    public class MovimentoValidator : AbstractValidator<Movimento>
    {
        public MovimentoValidator()
        {
            RuleFor(m => m.IdContaCorrente)
                .NotEqual(Guid.Empty).WithMessage("O Id da conta corrente não pode ser vazio.");

            RuleFor(m => m.TipoMovimento)
                .NotEmpty().WithMessage("O tipo de movimento não pode ser vazio.")
                .Length(1).WithMessage("O tipo de movimento deve ter 1 caractere (C para crédito ou D para débito).");

            RuleFor(m => m.Valor)
                .GreaterThan(0).WithMessage("O valor do movimento deve ser maior que zero.");
        }
    }
}
