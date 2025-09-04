
using FluentValidation;

namespace BankMore.Core.Entities.Validators
{
    public class ContaCorrenteValidator : AbstractValidator<ContaCorrente>
    {
        public ContaCorrenteValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome não pode ser vazio.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

            RuleFor(c => c.Senha)
                .NotEmpty().WithMessage("A senha não pode ser vazia.");

            RuleFor(c => c.Saldo)
                .GreaterThanOrEqualTo(0).WithMessage("O saldo não pode ser negativo.");
        }
    }
}
