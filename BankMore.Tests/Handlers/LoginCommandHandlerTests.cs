
using BankMore.Core.Commands;
using BankMore.Core.Entities;
using BankMore.Core.Handlers;
using BankMore.Core.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BankMore.Tests.Handlers
{
    public class LoginCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCredentials_ShouldReturnToken()
        {
            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(ph => ph.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var contaCorrente = new ContaCorrente("Test User", "password123", "salt", "test_salt");
            contaCorrenteRepositoryMock.Setup(r => r.GetByNumero(It.IsAny<int>()))
                .ReturnsAsync(contaCorrente);

            var handler = new LoginCommandHandler(contaCorrenteRepositoryMock.Object, passwordHasherMock.Object);
            var command = new LoginCommand
            {
                NumeroConta = contaCorrente.Numero,
                Senha = "password123"
            };

            var token = await handler.Handle(command, CancellationToken.None);

            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task Handle_InvalidCredentials_ShouldThrowUnauthorizedAccessException()
        {
            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(ph => ph.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            contaCorrenteRepositoryMock.Setup(r => r.GetByNumero(It.IsAny<int>()))
                .ReturnsAsync((ContaCorrente)null);

            var handler = new LoginCommandHandler(contaCorrenteRepositoryMock.Object, passwordHasherMock.Object);
            var command = new LoginCommand
            {
                NumeroConta = 12345,
                Senha = "wrongpassword"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidPassword_ShouldThrowUnauthorizedAccessException()
        {
            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(ph => ph.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var contaCorrente = new ContaCorrente("Test User", "password123", "salt", "test_salt");
            contaCorrenteRepositoryMock.Setup(r => r.GetByNumero(It.IsAny<int>()))
                .ReturnsAsync(contaCorrente);

            var handler = new LoginCommandHandler(contaCorrenteRepositoryMock.Object, passwordHasherMock.Object);
            var command = new LoginCommand
            {
                NumeroConta = contaCorrente.Numero,
                Senha = "wrongpassword"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
