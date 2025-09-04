
using System;

namespace BankMore.Core.Entities
{
    public class Transferencia
    {
        public Guid Id { get; private set; }
        public Guid IdContaCorrenteOrigem { get; private set; }
        public Guid IdContaCorrenteDestino { get; private set; }
        public DateTime DataTransferencia { get; private set; }
        public decimal Valor { get; private set; }

        public Transferencia(Guid idContaCorrenteOrigem, Guid idContaCorrenteDestino, decimal valor)
        {
            Id = Guid.NewGuid();
            IdContaCorrenteOrigem = idContaCorrenteOrigem;
            IdContaCorrenteDestino = idContaCorrenteDestino;
            DataTransferencia = DateTime.UtcNow;
            Valor = valor;
        }
    }
}
