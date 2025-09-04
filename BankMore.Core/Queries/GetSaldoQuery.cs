
using MediatR;
using System;

namespace BankMore.Core.Queries
{
    public class GetSaldoQuery : IRequest<decimal>
    {
        public Guid IdContaCorrente { get; set; }
    }
}
