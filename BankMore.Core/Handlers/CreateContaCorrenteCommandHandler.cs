using BankMore.Core.Commands;
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using MediatR;

namespace BankMore.Core.Handlers
{
    public class CreateContaCorrenteCommandHandler : IRequestHandler<CreateContaCorrenteCommand, int>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public CreateContaCorrenteCommandHandler(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<int> Handle(CreateContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            if (request.Cpf == null || !IsCpfValid(request.Cpf))
            {
                throw new ArgumentException("CPF inválido.");
            }

            if (string.IsNullOrEmpty(request.Nome) || string.IsNullOrEmpty(request.Senha))
            {
                throw new ArgumentException("Nome e senha são obrigatórios.");
            }

            var salt = "some-salt";
            var contaCorrente = new ContaCorrente(request.Nome, request.Cpf, request.Senha, salt);
            await _contaCorrenteRepository.Add(contaCorrente);

            return contaCorrente.Numero;
        }

        private bool IsCpfValid(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            if (cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999")
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}