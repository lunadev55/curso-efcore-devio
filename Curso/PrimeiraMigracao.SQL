IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Clientes] (
    [Id] int NOT NULL IDENTITY,
    [Nome] VARCHAR(80) NOT NULL,
    [Telefone] CHAR(11) NULL,
    [CEP] CHAR(8) NOT NULL,
    [Estado] CHAR(2) NOT NULL,
    [Cidade] nvarchar(60) NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
);

CREATE TABLE [Produtos] (
    [Id] int NOT NULL IDENTITY,
    [CodigoBarras] VARCHAR(14) NOT NULL,
    [Descricao] VARCHAR(60) NULL,
    [Valor] decimal(18,2) NOT NULL,
    [TipoProduto] nvarchar(max) NOT NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id])
);

CREATE TABLE [Pedidos] (
    [Id] int NOT NULL IDENTITY,
    [ClienteId] int NOT NULL,
    [IniciadoEm] datetime2 NOT NULL DEFAULT (GETDATE()),
    [FinalizadoEm] datetime2 NOT NULL,
    [TipoFrete] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Observacao] VARCHAR(512) NULL,
    CONSTRAINT [PK_Pedidos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Pedidos_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [PedidoItens] (
    [Id] int NOT NULL IDENTITY,
    [PedidoId] int NOT NULL,
    [ProdutoId] int NOT NULL,
    [Quantidade] int NOT NULL DEFAULT 1,
    [Valor] decimal(18,2) NOT NULL DEFAULT 0.0,
    [Desconto] decimal(18,2) NOT NULL DEFAULT 0.0,
    CONSTRAINT [PK_PedidoItens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PedidoItens_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PedidoItens_Produtos_ProdutoId] FOREIGN KEY ([ProdutoId]) REFERENCES [Produtos] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [idx_cliente_telefone] ON [Clientes] ([Telefone]);

CREATE INDEX [IX_PedidoItens_PedidoId] ON [PedidoItens] ([PedidoId]);

CREATE INDEX [IX_PedidoItens_ProdutoId] ON [PedidoItens] ([ProdutoId]);

CREATE INDEX [IX_Pedidos_ClienteId] ON [Pedidos] ([ClienteId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260216145141_PrimeiraMigracao', N'9.0.13');

COMMIT;
GO

