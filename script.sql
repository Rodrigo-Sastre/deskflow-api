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
CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);

CREATE TABLE [Chamados] (
    [Id] int NOT NULL IDENTITY,
    [Titulo] nvarchar(max) NOT NULL,
    [Descricao] nvarchar(max) NOT NULL,
    [Prioridade] int NOT NULL,
    [Status] int NOT NULL,
    [SolicitanteNome] nvarchar(max) NOT NULL,
    [DataAbertura] datetime2 NOT NULL,
    [DataFechamento] datetime2 NULL,
    [Solucao] nvarchar(max) NULL,
    [CategoriaId] int NOT NULL,
    CONSTRAINT [PK_Chamados] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Chamados_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Interacoes] (
    [Id] int NOT NULL IDENTITY,
    [Autor] nvarchar(max) NOT NULL,
    [Mensagem] nvarchar(max) NOT NULL,
    [DataRegistro] datetime2 NOT NULL,
    [ChamadoId] int NOT NULL,
    CONSTRAINT [PK_Interacoes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Interacoes_Chamados_ChamadoId] FOREIGN KEY ([ChamadoId]) REFERENCES [Chamados] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Chamados_CategoriaId] ON [Chamados] ([CategoriaId]);

CREATE INDEX [IX_Interacoes_ChamadoId] ON [Interacoes] ([ChamadoId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260922232406_CriacaoInicial', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Categorias] ADD [Ativo] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260924224433_AdicionaCamposEncerramentoChamado', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Interacoes] DROP CONSTRAINT [FK_Interacoes_Chamados_ChamadoId];

ALTER TABLE [Interacoes] DROP CONSTRAINT [PK_Interacoes];

DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Interacoes]') AND [c].[name] = N'Autor');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Interacoes] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [Interacoes] DROP COLUMN [Autor];

EXEC sp_rename N'[Interacoes]', N'Tb_Interacoes', 'OBJECT';

EXEC sp_rename N'[Tb_Interacoes].[IX_Interacoes_ChamadoId]', N'IX_Tb_Interacoes_ChamadoId', 'INDEX';

ALTER TABLE [Tb_Interacoes] ADD [QuemEscreveu] nvarchar(100) NOT NULL DEFAULT N'';

ALTER TABLE [Tb_Interacoes] ADD CONSTRAINT [PK_Tb_Interacoes] PRIMARY KEY ([Id]);

ALTER TABLE [Tb_Interacoes] ADD CONSTRAINT [FK_Tb_Interacoes_Chamados_ChamadoId] FOREIGN KEY ([ChamadoId]) REFERENCES [Chamados] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260925224423_CriacaoTabelaInteracao', N'10.0.12');

COMMIT;
GO

