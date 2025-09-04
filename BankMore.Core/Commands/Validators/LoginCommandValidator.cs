using FluentValidation;

namespace BankMore.Core.Commands.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(c => c.Senha)
                .NotEmpty().WithMessage("A senha não pode ser vazia.");

            When(c => c.NumeroConta == null, () =>
            {
                RuleFor(c => c.Cpf)
                    .NotEmpty().WithMessage("O CPF é obrigatório se o número da conta não for informado.")
                    .Length(11).WithMessage("O CPF deve ter 11 caracteres.");
            });

            When(c => string.IsNullOrEmpty(c.Cpf), () =>
            {
                RuleFor(c => c.NumeroConta)
                    .NotNull().WithMessage("O número da conta é obrigatório se o CPF não for informado.");
            });
        }
    }
}
