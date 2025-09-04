using FluentValidation;

namespace BankMore.Core.Commands.Validators
{
    public class CreateContaCorrenteCommandValidator : AbstractValidator<CreateContaCorrenteCommand>
    {
        public CreateContaCorrenteCommandValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome não pode ser vazio.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

            RuleFor(c => c.Senha)
                .NotEmpty().WithMessage("A senha não pode ser vazia.");

            RuleFor(c => c.Cpf)
                .NotEmpty().WithMessage("O CPF não pode ser vazio.")
                .Length(11).WithMessage("O CPF deve ter 11 caracteres.");
        }
    }
}
