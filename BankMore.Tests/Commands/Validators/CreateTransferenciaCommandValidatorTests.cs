using BankMore.Core.Commands;
using BankMore.Core.Commands.Validators;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace BankMore.Tests.Commands.Validators
{
    public class CreateTransferenciaCommandValidatorTests
    {
        private readonly CreateTransferenciaCommandValidator _validator;

        public CreateTransferenciaCommandValidatorTests()
        {
            _validator = new CreateTransferenciaCommandValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenIdRequisicaoIsEmpty()
        {
            var command = new CreateTransferenciaCommand { IdRequisicao = Guid.Empty };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.IdRequisicao);
        }

        [Fact]
        public void ShouldHaveErrorWhenIdContaCorrenteOrigemIsEmpty()
        {
            var command = new CreateTransferenciaCommand { IdContaCorrenteOrigem = Guid.Empty };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.IdContaCorrenteOrigem);
        }

        [Fact]
        public void ShouldHaveErrorWhenIdContaCorrenteDestinoIsEmpty()
        {
            var command = new CreateTransferenciaCommand { IdContaCorrenteDestino = Guid.Empty };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.IdContaCorrenteDestino);
        }

        [Fact]
        public void ShouldHaveErrorWhenValorIsZeroOrNegative()
        {
            var command = new CreateTransferenciaCommand { Valor = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Valor);

            command.Valor = -10;
            result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Valor);
        }

        [Fact]
        public void ShouldNotHaveErrorWhenCommandIsValid()
        {
            var command = new CreateTransferenciaCommand
            {
                IdRequisicao = Guid.NewGuid(),
                IdContaCorrenteOrigem = Guid.NewGuid(),
                IdContaCorrenteDestino = Guid.NewGuid(),
                Valor = 100m
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}