using FluentValidation;
using System;

namespace BankMore.Core.Queries.Validators
{
    public class GetSaldoQueryValidator : AbstractValidator<GetSaldoQuery>
    {
        public GetSaldoQueryValidator()
        {
            RuleFor(q => q.IdContaCorrente)
                .NotEqual(Guid.Empty).WithMessage("O Id da conta corrente não pode ser vazio.");
        }
    }
}
