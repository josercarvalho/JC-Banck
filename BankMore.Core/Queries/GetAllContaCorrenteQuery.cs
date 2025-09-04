using MediatR;
using BankMore.Core.Entities;
using System.Collections.Generic;

namespace BankMore.Core.Queries
{
    public class GetAllContaCorrenteQuery : IRequest<IEnumerable<ContaCorrente>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllContaCorrenteQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}