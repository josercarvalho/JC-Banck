CREATE TABLE IF NOT EXISTS ContaCorrente (
    Id UUID PRIMARY KEY,
    Numero INT,
    Nome VARCHAR(100),
    Ativo BOOLEAN,
    Senha TEXT,
    Salt TEXT,
    Saldo DECIMAL(18, 2)
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