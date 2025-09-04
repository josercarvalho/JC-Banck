
using MediatR;
using System;

namespace BankMore.Core.Commands
{
    public class CreateTransferenciaCommand : IRequest
    {
        public Guid IdRequisicao { get; set; }
        public Guid IdContaCorrenteOrigem { get; set; }
        public Guid IdContaCorrenteDestino { get; set; }
        public decimal Valor { get; set; }
    }
}
