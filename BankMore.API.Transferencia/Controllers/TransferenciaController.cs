using BankMore.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BankMore.API.Transferencia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TransferenciaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransferenciaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransferenciaCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message, type = "INVALID_DATA" });
            }
        }
    }
}