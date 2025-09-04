
-- Table: ContaCorrente
CREATE TABLE IF NOT EXISTS ContaCorrente (
    Id TEXT PRIMARY KEY,
    Numero INTEGER,
    Nome TEXT,
    Ativo INTEGER,
    Senha TEXT,
    Salt TEXT,
    Saldo REAL
);

-- Table: Movimento
CREATE TABLE IF NOT EXISTS Movimento (
    Id TEXT PRIMARY KEY,
    IdContaCorrente TEXT,
    DataMovimento TEXT,
    TipoMovimento TEXT,
    Valor REAL
);

-- Table: Transferencia
CREATE TABLE IF NOT EXISTS Transferencia (
    Id TEXT PRIMARY KEY,
    IdContaCorrenteOrigem TEXT,
    IdContaCorrenteDestino TEXT,
    DataTransferencia TEXT,
    Valor REAL
);

-- Table: Idempotencia
CREATE TABLE IF NOT EXISTS Idempotencia (
    ChaveIdempotencia TEXT PRIMARY KEY,
    Requisicao TEXT,
    Resultado TEXT
);
