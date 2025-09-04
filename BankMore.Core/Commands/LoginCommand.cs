using MediatR;

namespace BankMore.Core.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public int? NumeroConta { get; set; }
        public string? Cpf { get; set; }
        public string? Senha { get; set; }
    }
}