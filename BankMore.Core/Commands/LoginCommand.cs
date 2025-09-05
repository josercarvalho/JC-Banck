using MediatR;

namespace BankMore.Core.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public string? NumeroConta { get; set; }
        public string? Cpf { get; set; }
        public string Senha { get; set; }
    }
}