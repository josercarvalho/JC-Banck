
using BankMore.Core.Entities.Abstractions;
using BankMore.Core.Entities.Validators;

namespace BankMore.Core.Entities
{
    public class ContaCorrente : Entity
    {
        public int Numero { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
        public string Senha { get; private set; }
        public string Salt { get; private set; }
        public decimal Saldo { get; private set; }

        public ContaCorrente(string nome, string senha, string salt)
        {
            Numero = new Random().Next(10000, 99999);
            Nome = nome;
            Ativo = true;
            Senha = senha;
            Salt = salt;
            Saldo = 0;
        }

        public void Inativar()
        {
            Ativo = false;
        }

        public void Debitar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do débito deve ser positivo.");

            if (Saldo < valor)
                throw new InvalidOperationException("Saldo insuficiente.");

            Saldo -= valor;
        }

        public void Creditar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do crédito deve ser positivo.");

            Saldo += valor;
        }

        public override bool Validate()
        {
            ValidationResult = new ContaCorrenteValidator().Validate(this);
            return IsValid;
        }
    }
}
