using FluentValidation;
using System;

namespace BankMore.Core.Commands.Validators
{
    public class InativarContaCorrenteCommandValidator : AbstractValidator<InativarContaCorrenteCommand>
    {
        public InativarContaCorrenteCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("O Id da conta não pode ser vazio.");

            RuleFor(c => c.Senha)
                .NotEmpty().WithMessage("A senha não pode ser vazia.");
        }
    }
}
