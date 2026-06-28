USE TesteDevCSharp;
GO

IF OBJECT_ID('dbo.Enderecos', 'U') IS NOT NULL
    DROP TABLE dbo.Enderecos;
GO

IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL
    DROP TABLE dbo.Usuarios;
GO

CREATE TABLE Usuarios
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Usuario VARCHAR(50) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL
);
GO

CREATE TABLE Enderecos
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CEP VARCHAR(9) NOT NULL,
    Logradouro VARCHAR(200) NOT NULL,
    Complemento VARCHAR(200) NULL,
    Bairro VARCHAR(100) NOT NULL,
    Cidade VARCHAR(100) NOT NULL,
    UF CHAR(2) NOT NULL,
    Numero VARCHAR(20) NOT NULL,
    UsuarioId INT NOT NULL,
    CONSTRAINT FK_Enderecos_Usuarios
        FOREIGN KEY (UsuarioId)
        REFERENCES Usuarios(Id)
);
GO