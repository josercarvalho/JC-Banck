using BankMore.Core.Commands;
using BankMore.Core.Entities;
using BankMore.Core.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankMore.Core.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IContaCorrenteRepository contaCorrenteRepository, IPasswordHasher passwordHasher)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _passwordHasher = passwordHasher;
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
                contaCorrente = await _contaCorrenteRepository.GetByCpf(request.Cpf);
            }
            else
            {
                throw new ArgumentException("Número da conta ou CPF devem ser informados.");
            }
            
            if (contaCorrente == null || !contaCorrente.Ativo)
            {
                throw new System.UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            var isPasswordValid = _passwordHasher.VerifyPassword(request.Senha, contaCorrente.Senha, contaCorrente.Salt);
            if (!isPasswordValid)
            {
                throw new System.UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "thisisalongsecretkeyforjwttokengeneration";
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