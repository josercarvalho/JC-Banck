
using BankMore.Core.Entities.Validators;
using FluentValidation.Results;
using System;

namespace BankMore.Core.Entities
{
    public class Idempotencia
    {
        public Guid ChaveIdempotencia { get; private set; }
        public string Requisicao { get; private set; }
        public string Resultado { get; private set; }

        public bool IsValid => ValidationResult.IsValid;
        public bool IsInvalid => !IsValid;

        protected ValidationResult ValidationResult { get; set; }

        public Idempotencia(Guid chaveIdempotencia, string requisicao, string resultado)
        {
            ChaveIdempotencia = chaveIdempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
            ValidationResult = new ValidationResult();
        }

        public bool Validate()
        {
            ValidationResult = new IdempotenciaValidator().Validate(this);
            return IsValid;
        }
    }
}
