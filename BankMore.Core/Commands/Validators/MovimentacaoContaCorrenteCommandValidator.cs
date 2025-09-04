using FluentValidation;
using System;

namespace BankMore.Core.Commands.Validators
{
    public class MovimentacaoContaCorrenteCommandValidator : AbstractValidator<MovimentacaoContaCorrenteCommand>
    {
        public MovimentacaoContaCorrenteCommandValidator()
        {
            RuleFor(c => c.IdRequisicao)
                .NotEqual(Guid.Empty).WithMessage("O Id da requisição não pode ser vazio.");

            RuleFor(c => c.IdContaCorrente)
                .NotEqual(Guid.Empty).WithMessage("O Id da conta corrente não pode ser vazio.");

            RuleFor(c => c.Valor)
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");

            RuleFor(c => c.TipoMovimento)
                .NotEmpty().WithMessage("O tipo de movimento não pode ser vazio.")
                .Must(t => t == "C" || t == "D").WithMessage("O tipo de movimento deve ser 'C' para crédito ou 'D' para débito.");
        }
    }
}
