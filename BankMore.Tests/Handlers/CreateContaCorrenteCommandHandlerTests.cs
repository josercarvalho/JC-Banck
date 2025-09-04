using BankMore.Core.Commands;
using BankMore.Core.Handlers;
using BankMore.Core.Interfaces;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BankMore.Tests.Handlers
{
    public class CreateContaCorrenteCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCpf_ShouldCreateContaCorrente()
        {
            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var handler = new CreateContaCorrenteCommandHandler(contaCorrenteRepositoryMock.Object);
            var command = new CreateContaCorrenteCommand
            {
                Nome = "Test User",
                Senha = "123456",
                Cpf = "47355309026"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(0, result);
            contaCorrenteRepositoryMock.Verify(r => r.Add(It.IsAny<BankMore.Core.Entities.ContaCorrente>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCpf_ShouldThrowArgumentException()
        {
            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var handler = new CreateContaCorrenteCommandHandler(contaCorrenteRepositoryMock.Object);
            var command = new CreateContaCorrenteCommand
            {
                Nome = "Test User",
                Senha = "123456",
                Cpf = "12345678900"
            };

            await Assert.ThrowsAsync<System.ArgumentException>(() => handler.Handle(command, CancellationToken.None));
            contaCorrenteRepositoryMock.Verify(r => r.Add(It.IsAny<BankMore.Core.Entities.ContaCorrente>()), Times.Never);
        }

        [Fact]
        public async Task Handle_AllEqualDigitsCpf_ShouldThrowArgumentException()
        {
            var contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            var handler = new CreateContaCorrenteCommandHandler(contaCorrenteRepositoryMock.Object);
            var command = new CreateContaCorrenteCommand
            {
                Nome = "Test User",
                Senha = "123456",
                Cpf = "11111111111"
            };

            await Assert.ThrowsAsync<System.ArgumentException>(() => handler.Handle(command, CancellationToken.None));
            contaCorrenteRepositoryMock.Verify(r => r.Add(It.IsAny<BankMore.Core.Entities.ContaCorrente>()), Times.Never);
        }
    }
}