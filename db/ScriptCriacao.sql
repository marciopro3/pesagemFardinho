-- ========================================
-- Criação da tabela de operadores
-- ========================================
CREATE TABLE IF NOT EXISTS Operador (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome TEXT NOT NULL,
    Login TEXT NOT NULL UNIQUE,
    Senha TEXT NOT NULL
);

-- ========================================
-- Criação da tabela de pesagens
-- ========================================
CREATE TABLE IF NOT EXISTS Pesagem (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    DataHora TEXT NOT NULL,
    CodigoDeBarras TEXT NOT NULL,
    NumeroOperacional TEXT NOT NULL,
    PesoBruto REAL NOT NULL,
    Tempo TEXT NOT NULL,
    OperadorId INTEGER NOT NULL,
    FOREIGN KEY (OperadorId) REFERENCES Operador(Id)
);

-- ========================================
-- Inserção do operador Márcio Soares
-- ========================================
INSERT INTO Operador (Nome, Login, Senha)
VALUES ('Márcio Soares', 'marcio.soares@grupojcn.com.br', '123');

-- ========================================
-- Inserção de exemplo na tabela Pesagem
-- ========================================
-- Código de barras completo: 00078985390716427507
-- Número operacional extraído previamente: 1642750

INSERT INTO Pesagem (
    DataHora, CodigoDeBarras, NumeroOperacional,
    PesoBruto, Tempo, OperadorId
)
VALUES (
    datetime('now', '-3 hours'),       -- DataHora com horário de Brasília
    '00078985390716427507',            -- CodigoDeBarras
    '1642750',                         -- NumeroOperacional (fixo, sem SUBSTR)
    1250.50,                           -- PesoBruto
    '00:03:42',                        -- Tempo
    1                                  -- OperadorId (referência ao Márcio)
);
