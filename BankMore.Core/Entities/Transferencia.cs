
using BankMore.Core.Entities.Abstractions;
using BankMore.Core.Entities.Validators;
using System;

namespace BankMore.Core.Entities
{
    public class Transferencia : Entity
    {
        public Guid IdContaCorrenteOrigem { get; private set; }
        public Guid IdContaCorrenteDestino { get; private set; }
        public DateTime DataTransferencia { get; private set; }
        public decimal Valor { get; private set; }

        public Transferencia(Guid idContaCorrenteOrigem, Guid idContaCorrenteDestino, decimal valor)
        {
            IdContaCorrenteOrigem = idContaCorrenteOrigem;
            IdContaCorrenteDestino = idContaCorrenteDestino;
            DataTransferencia = DateTime.UtcNow;
            Valor = valor;
        }

        public override bool Validate()
        {
            ValidationResult = new TransferenciaValidator().Validate(this);
            return IsValid;
        }
    }
}
