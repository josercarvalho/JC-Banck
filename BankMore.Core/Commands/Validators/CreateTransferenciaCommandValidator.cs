using FluentValidation;
using System;

namespace BankMore.Core.Commands.Validators
{
    public class CreateTransferenciaCommandValidator : AbstractValidator<CreateTransferenciaCommand>
    {
        public CreateTransferenciaCommandValidator()
        {
            RuleFor(c => c.IdRequisicao)
                .NotEqual(Guid.Empty).WithMessage("O Id da requisição não pode ser vazio.");

            RuleFor(c => c.IdContaCorrenteOrigem)
                .NotEqual(Guid.Empty).WithMessage("O Id da conta de origem não pode ser vazio.");

            RuleFor(c => c.IdContaCorrenteDestino)
                .NotEqual(Guid.Empty).WithMessage("O Id da conta de destino não pode ser vazio.");

            RuleFor(c => c.Valor)
                .GreaterThan(0).WithMessage("O valor da transferência deve ser maior que zero.");
        }
    }
}
