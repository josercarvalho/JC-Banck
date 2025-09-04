using MediatR;
using System;

namespace BankMore.Core.Commands
{
    public class MovimentacaoContaCorrenteCommand : IRequest
    {
        public Guid IdRequisicao { get; set; }
        public Guid? IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string? TipoMovimento { get; set; }
    }
}