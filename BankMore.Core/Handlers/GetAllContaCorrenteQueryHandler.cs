using MediatR;
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using BankMore.Core.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BankMore.Core.Handlers
{
    public class GetAllContaCorrenteQueryHandler : IRequestHandler<GetAllContaCorrenteQuery, IEnumerable<ContaCorrente>>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public GetAllContaCorrenteQueryHandler(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<IEnumerable<ContaCorrente>> Handle(GetAllContaCorrenteQuery request, CancellationToken cancellationToken)
        {
            return await _contaCorrenteRepository.GetAll(request.PageNumber, request.PageSize);
        }
    }
}