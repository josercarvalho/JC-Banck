
using BankMore.Core.Entities.Abstractions;
using BankMore.Core.Entities.Validators;
using System;

namespace BankMore.Core.Entities
{
    public class Movimento : Entity
    {
        public Guid IdContaCorrente { get; private set; }
        public DateTime DataMovimento { get; private set; }
        public string TipoMovimento { get; private set; }
        public decimal Valor { get; private set; }

        public Movimento(Guid idContaCorrente, string tipoMovimento, decimal valor)
        {
            IdContaCorrente = idContaCorrente;
            DataMovimento = DateTime.UtcNow;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }

        public override bool Validate()
        {
            ValidationResult = new MovimentoValidator().Validate(this);
            return IsValid;
        }
    }
}
