CREATE TABLE ContaCorrente (
    Id UUID PRIMARY KEY,
    NumeroConta VARCHAR(20) NOT NULL UNIQUE,
    Nome VARCHAR(100),
    CPF VARCHAR(14) NOT NULL UNIQUE,
    Saldo DECIMAL(18, 2) NOT NULL,
    Ativa BOOLEAN NOT NULL,
    Senha VARCHAR(100) NOT NULL, 
    Salt VARCHAR(100) NOT NULL,
    DataCriacao TIMESTAMP NOT NULL,
    DataAtualizacao TIMESTAMP NOT NULL
);

CREATE TABLE IF NOT EXISTS Movimento (
    Id UUID PRIMARY KEY,
    IdContaCorrente UUID,
    DataMovimento TIMESTAMP,
    TipoMovimento VARCHAR(1),
    Valor DECIMAL(18, 2),
    FOREIGN KEY(IdContaCorrente) REFERENCES ContaCorrente(Id)
);

CREATE TABLE IF NOT EXISTS Transferencia (
    Id UUID PRIMARY KEY,
    IdContaCorrenteOrigem UUID,
    IdContaCorrenteDestino UUID,
    DataTransferencia TIMESTAMP,
    Valor DECIMAL(18, 2),
    FOREIGN KEY(IdContaCorrenteOrigem) REFERENCES ContaCorrente(Id),
    FOREIGN KEY(IdContaCorrenteDestino) REFERENCES ContaCorrente(Id)
);

CREATE TABLE IF NOT EXISTS Idempotencia (
    ChaveIdempotencia UUID PRIMARY KEY,
    Requisicao TEXT,
    Resultado TEXT
);

CREATE TABLE IF NOT EXISTS Tarifa (
    IdTarifa UUID PRIMARY KEY,
    IdContaCorrente UUID,
    DataMovimento TIMESTAMP,
    Valor DECIMAL(18, 2),
    FOREIGN KEY(IdContaCorrente) REFERENCES ContaCorrente(Id)
);