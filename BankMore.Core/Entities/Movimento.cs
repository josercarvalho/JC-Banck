
using System;

namespace BankMore.Core.Entities
{
    public class Movimento
    {
        public Guid Id { get; private set; }
        public Guid IdContaCorrente { get; private set; }
        public DateTime DataMovimento { get; private set; }
        public string TipoMovimento { get; private set; }
        public decimal Valor { get; private set; }

        public Movimento(Guid idContaCorrente, string tipoMovimento, decimal valor)
        {
            Id = Guid.NewGuid();
            IdContaCorrente = idContaCorrente;
            DataMovimento = DateTime.UtcNow;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }
    }
}
