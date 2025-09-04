using BankMore.Core.Commands;
using BankMore.Core.Entities;
using BankMore.Core.Handlers;
using BankMore.Core.Interfaces;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BankMore.Tests.Handlers
{
    public class CreateTransferenciaCommandHandlerTests
    {
        private readonly Mock<ITransferenciaRepository> _transferenciaRepositoryMock;
        private readonly Mock<IIdempotenciaRepository> _idempotenciaRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CreateTransferenciaCommandHandler _handler;

        public CreateTransferenciaCommandHandlerTests()
        {
            _transferenciaRepositoryMock = new Mock<ITransferenciaRepository>();
            _idempotenciaRepositoryMock = new Mock<IIdempotenciaRepository>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new CreateTransferenciaCommandHandler(
                _transferenciaRepositoryMock.Object,
                _idempotenciaRepositoryMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_SuccessfulTransfer_ShouldReturnUnitValue()
        {
            // Arrange
            var idRequisicao = Guid.NewGuid();
            var idContaCorrenteOrigem = Guid.NewGuid();
            var idContaCorrenteDestino = Guid.NewGuid();
            var valor = 100m;

            var command = new CreateTransferenciaCommand
            {
                IdRequisicao = idRequisicao,
                IdContaCorrenteOrigem = idContaCorrenteOrigem,
                IdContaCorrenteDestino = idContaCorrenteDestino,
                Valor = valor
            };

            _idempotenciaRepositoryMock.Setup(r => r.GetById(idRequisicao)).ReturnsAsync((Idempotencia)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _idempotenciaRepositoryMock.Verify(r => r.Add(It.Is<Idempotencia>(i => i.ChaveIdempotencia == idRequisicao)), Times.Once);
            _transferenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Transferencia>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(
                It.Is<MovimentacaoContaCorrenteCommand>(cmd =>
                    cmd.IdRequisicao == idRequisicao &&
                    cmd.IdContaCorrente == idContaCorrenteOrigem &&
                    cmd.Valor == valor &&
                    cmd.TipoMovimento == "D"),
                It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(
                It.Is<MovimentacaoContaCorrenteCommand>(cmd =>
                    cmd.IdRequisicao == idRequisicao &&
                    cmd.IdContaCorrente == idContaCorrenteDestino &&
                    cmd.Valor == valor &&
                    cmd.TipoMovimento == "C"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_OriginAccountNotFound_ShouldThrowArgumentException()
        {
            // Arrange
            var idRequisicao = Guid.NewGuid();
            var idContaCorrenteOrigem = Guid.NewGuid();
            var idContaCorrenteDestino = Guid.NewGuid();
            var valor = 100m;

            var command = new CreateTransferenciaCommand
            {
                IdRequisicao = idRequisicao,
                IdContaCorrenteOrigem = idContaCorrenteOrigem,
                IdContaCorrenteDestino = idContaCorrenteDestino,
                Valor = valor
            };

            _idempotenciaRepositoryMock.Setup(r => r.GetById(idRequisicao)).ReturnsAsync((Idempotencia)null);
            _mediatorMock.Setup(m => m.Send(
                It.Is<MovimentacaoContaCorrenteCommand>(cmd => cmd.IdContaCorrente == idContaCorrenteOrigem),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Conta de origem não encontrada."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Conta de origem não encontrada.", exception.Message);
            _idempotenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Idempotencia>()), Times.Never);
            _transferenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Transferencia>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DestinationAccountNotFound_ShouldThrowArgumentException()
        {
            // Arrange
            var idRequisicao = Guid.NewGuid();
            var idContaCorrenteOrigem = Guid.NewGuid();
            var idContaCorrenteDestino = Guid.NewGuid();
            var valor = 100m;

            var command = new CreateTransferenciaCommand
            {
                IdRequisicao = idRequisicao,
                IdContaCorrenteOrigem = idContaCorrenteOrigem,
                IdContaCorrenteDestino = idContaCorrenteDestino,
                Valor = valor
            };

            _idempotenciaRepositoryMock.Setup(r => r.GetById(idRequisicao)).ReturnsAsync((Idempotencia)null);
            _mediatorMock.Setup(m => m.Send(
                It.Is<MovimentacaoContaCorrenteCommand>(cmd => cmd.IdContaCorrente == idContaCorrenteDestino),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Conta de destino não encontrada."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Conta de destino não encontrada.", exception.Message);
            _idempotenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Idempotencia>()), Times.Never);
            _transferenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Transferencia>()), Times.Never);

            // Verify that the debit was reversed
            _mediatorMock.Verify(m => m.Send(
                It.Is<MovimentacaoContaCorrenteCommand>(cmd =>
                    cmd.IdRequisicao == idRequisicao &&
                    cmd.IdContaCorrente == idContaCorrenteOrigem &&
                    cmd.Valor == valor &&
                    cmd.TipoMovimento == "C"), // Reversal is a credit
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InsufficientFunds_ShouldThrowArgumentException()
        {
            // Arrange
            var idRequisicao = Guid.NewGuid();
            var idContaCorrenteOrigem = Guid.NewGuid();
            var idContaCorrenteDestino = Guid.NewGuid();
            var valor = 100m;

            var command = new CreateTransferenciaCommand
            {
                IdRequisicao = idRequisicao,
                IdContaCorrenteOrigem = idContaCorrenteOrigem,
                IdContaCorrenteDestino = idContaCorrenteDestino,
                Valor = valor
            };

            _idempotenciaRepositoryMock.Setup(r => r.GetById(idRequisicao)).ReturnsAsync((Idempotencia)null);
            _mediatorMock.Setup(m => m.Send(
                It.Is<MovimentacaoContaCorrenteCommand>(cmd => cmd.IdContaCorrente == idContaCorrenteOrigem),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Saldo insuficiente na conta de origem."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Saldo insuficiente na conta de origem.", exception.Message);
            _idempotenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Idempotencia>()), Times.Never);
            _transferenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Transferencia>()), Times.Never);
        }

        [Fact]
        public async Task Handle_IdempotentRequestAlreadyProcessed_ShouldReturnUnitValue()
        {
            // Arrange
            var idRequisicao = Guid.NewGuid();
            var existingTransferenciaId = Guid.NewGuid();

            var command = new CreateTransferenciaCommand
            {
                IdRequisicao = idRequisicao,
                IdContaCorrenteOrigem = Guid.NewGuid(),
                IdContaCorrenteDestino = Guid.NewGuid(),
                Valor = 100m
            };

            _idempotenciaRepositoryMock.Setup(r => r.GetById(idRequisicao))
                .ReturnsAsync(new Idempotencia(idRequisicao, "", existingTransferenciaId.ToString()));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _idempotenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Idempotencia>()), Times.Never);
            _transferenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Transferencia>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<MovimentacaoContaCorrenteCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_IdempotentRequestInProgress_ShouldReturnUnitValue()
        {
            // Arrange
            var idRequisicao = Guid.NewGuid();

            var command = new CreateTransferenciaCommand
            {
                IdRequisicao = idRequisicao,
                IdContaCorrenteOrigem = Guid.NewGuid(),
                IdContaCorrenteDestino = Guid.NewGuid(),
                Valor = 100m
            };

            _idempotenciaRepositoryMock.Setup(r => r.GetById(idRequisicao))
                .ReturnsAsync(new Idempotencia(idRequisicao, "PROCESSANDO", ""));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _idempotenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Idempotencia>()), Times.Never);
            _transferenciaRepositoryMock.Verify(r => r.Add(It.IsAny<Transferencia>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<MovimentacaoContaCorrenteCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}