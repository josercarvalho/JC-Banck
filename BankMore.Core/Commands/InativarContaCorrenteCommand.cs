using MediatR;
using System;

namespace BankMore.Core.Commands
{
    public class InativarContaCorrenteCommand : IRequest
    {
        public Guid Id { get; set; }
        public string? Senha { get; set; }
    }
}