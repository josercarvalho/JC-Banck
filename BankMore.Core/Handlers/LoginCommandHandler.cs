using BankMore.Core.Commands;
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankMore.Core.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public LoginCommandHandler(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            ContaCorrente? contaCorrente = null;
            if (request.NumeroConta.HasValue)
            {
                contaCorrente = await _contaCorrenteRepository.GetByNumero(request.NumeroConta.Value);
            }
            else if (!string.IsNullOrEmpty(request.Cpf))
            {
                throw new System.NotImplementedException("Login with CPF is not implemented.");
            }

            if (contaCorrente == null || contaCorrente.Senha != request.Senha)
            {
                throw new System.UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "your-super-secret-key-for-development-only";
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, contaCorrente.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}