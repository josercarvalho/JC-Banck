using BankMore.Core.Commands;
using BankMore.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankMore.API.ContaCorrente.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContaCorrenteCommand command)
        {
            try
            {
                var numeroConta = await _mediator.Send(command);
                return Ok(new { numeroConta });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message, type = "INVALID_DOCUMENT" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            try
            {
                var token = await _mediator.Send(command);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message, type = "USER_UNAUTHORIZED" });
            }
        }

        [HttpPut("inativar")]
        [Authorize]
        public async Task<IActionResult> Inativar(InativarContaCorrenteCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message, type = "USER_UNAUTHORIZED" });
            }
        }

        [HttpPost("movimentacao")]
        [Authorize]
        public async Task<IActionResult> Movimentacao(MovimentacaoContaCorrenteCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, type = "INVALID_DATA" });
            }
        }

        [HttpGet("saldo")]
        [Authorize]
        public async Task<IActionResult> GetSaldo([FromQuery] GetSaldoQuery query)
        {
            try
            {
                var saldo = await _mediator.Send(query);
                return Ok(new { saldo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, type = "INVALID_DATA" });
            }
        }
    }
}