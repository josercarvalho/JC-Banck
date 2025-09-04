
using System;

namespace BankMore.Core.Entities
{
    public class Idempotencia
    {
        public Guid ChaveIdempotencia { get; private set; }
        public string Requisicao { get; private set; }
        public string Resultado { get; private set; }

        public Idempotencia(Guid chaveIdempotencia, string requisicao, string resultado)
        {
            ChaveIdempotencia = chaveIdempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
        }
    }
}
