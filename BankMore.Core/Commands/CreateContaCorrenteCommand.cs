using MediatR;

namespace BankMore.Core.Commands
{
    public class CreateContaCorrenteCommand : IRequest<int>
    {
        public string? Nome { get; set; }
        public string? Senha { get; set; }
        public string? Cpf { get; set; }
    }
}