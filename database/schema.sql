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

CREATE TABLE [clientes] (
    [id_cliente] int NOT NULL IDENTITY,
    [run] varchar(15) NOT NULL,
    [nombre] varchar(50) NOT NULL,
    [telefono] varchar(15) NULL,
    [email] varchar(100) NOT NULL,
    [direccion] varchar(100) NULL,
    CONSTRAINT [PK__clientes__677F38F5448FD6C5] PRIMARY KEY ([id_cliente])
);
GO

CREATE TABLE [especies] (
    [id_especie] int NOT NULL IDENTITY,
    [nombre_especie] varchar(30) NOT NULL,
    CONSTRAINT [PK__especies__96DDB0B982955345] PRIMARY KEY ([id_especie])
);
GO

CREATE TABLE [estado_consultas] (
    [id_estado_consulta] int NOT NULL IDENTITY,
    [nombre_estado] varchar(25) NOT NULL,
    CONSTRAINT [PK__estado_c__19A53235F51D74B3] PRIMARY KEY ([id_estado_consulta])
);
GO

CREATE TABLE [estado_usuarios] (
    [id_estado_usuario] int NOT NULL IDENTITY,
    [nombre_estado] varchar(25) NOT NULL,
    CONSTRAINT [PK__estado_u__CEFB9B89DD0EA143] PRIMARY KEY ([id_estado_usuario])
);
GO

CREATE TABLE [razas] (
    [id_raza] int NOT NULL IDENTITY,
    [nombre_raza] varchar(30) NOT NULL,
    CONSTRAINT [PK__razas__084F250A6A7054C8] PRIMARY KEY ([id_raza])
);
GO

CREATE TABLE [roles] (
    [id_rol] int NOT NULL IDENTITY,
    [nombre_rol] varchar(25) NOT NULL,
    CONSTRAINT [PK__roles__6ABCB5E0E52DCD0F] PRIMARY KEY ([id_rol])
);
GO

CREATE TABLE [mascotas] (
    [id_mascota] int NOT NULL IDENTITY,
    [id_cliente] int NOT NULL,
    [id_especie] int NOT NULL,
    [id_raza] int NOT NULL,
    [nombre] varchar(50) NOT NULL,
    [sexo] char(1) NULL,
    [edad] int NULL,
    CONSTRAINT [PK__mascotas__6F0373524828A893] PRIMARY KEY ([id_mascota]),
    CONSTRAINT [FK__mascotas__id_cli__4CA06362] FOREIGN KEY ([id_cliente]) REFERENCES [clientes] ([id_cliente]),
    CONSTRAINT [FK__mascotas__id_esp__4D94879B] FOREIGN KEY ([id_especie]) REFERENCES [especies] ([id_especie]),
    CONSTRAINT [FK__mascotas__id_raz__4E88ABD4] FOREIGN KEY ([id_raza]) REFERENCES [razas] ([id_raza])
);
GO

CREATE TABLE [usuarios] (
    [id_usuario] int NOT NULL IDENTITY,
    [id_rol] int NOT NULL,
    [id_estado_usuario] int NOT NULL,
    [nombre] varchar(25) NOT NULL,
    [email] varchar(100) NOT NULL,
    [password] varchar(10) NOT NULL,
    CONSTRAINT [PK__usuarios__4E3E04AD4CB7E388] PRIMARY KEY ([id_usuario]),
    CONSTRAINT [FK__usuarios__id_est__47DBAE45] FOREIGN KEY ([id_estado_usuario]) REFERENCES [estado_usuarios] ([id_estado_usuario]),
    CONSTRAINT [FK__usuarios__id_rol__46E78A0C] FOREIGN KEY ([id_rol]) REFERENCES [roles] ([id_rol])
);
GO

CREATE TABLE [consultas] (
    [id_consulta] int NOT NULL IDENTITY,
    [id_mascota] int NOT NULL,
    [id_usuario] int NOT NULL,
    [id_estado_consulta] int NOT NULL,
    [fecha_consulta] date NOT NULL,
    [motivo] text NULL,
    [diagnostico] text NULL,
    CONSTRAINT [PK__consulta__6F53588BC63A384F] PRIMARY KEY ([id_consulta]),
    CONSTRAINT [FK__consultas__id_es__534D60F1] FOREIGN KEY ([id_estado_consulta]) REFERENCES [estado_consultas] ([id_estado_consulta]),
    CONSTRAINT [FK__consultas__id_ma__5165187F] FOREIGN KEY ([id_mascota]) REFERENCES [mascotas] ([id_mascota]),
    CONSTRAINT [FK__consultas__id_us__52593CB8] FOREIGN KEY ([id_usuario]) REFERENCES [usuarios] ([id_usuario])
);
GO

CREATE TABLE [tratamientos] (
    [id_tratamiento] int NOT NULL IDENTITY,
    [id_consulta] int NOT NULL,
    [descripcion] varchar(255) NULL,
    [medicamento] varchar(100) NULL,
    CONSTRAINT [PK__tratamie__C8825F4CB6B1F22C] PRIMARY KEY ([id_tratamiento]),
    CONSTRAINT [FK__tratamien__id_co__5629CD9C] FOREIGN KEY ([id_consulta]) REFERENCES [consultas] ([id_consulta])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_estado_usuario', N'nombre_estado') AND [object_id] = OBJECT_ID(N'[estado_usuarios]'))
    SET IDENTITY_INSERT [estado_usuarios] ON;
INSERT INTO [estado_usuarios] ([id_estado_usuario], [nombre_estado])
VALUES (1, 'Activo'),
(2, 'Inactivo');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_estado_usuario', N'nombre_estado') AND [object_id] = OBJECT_ID(N'[estado_usuarios]'))
    SET IDENTITY_INSERT [estado_usuarios] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_rol', N'nombre_rol') AND [object_id] = OBJECT_ID(N'[roles]'))
    SET IDENTITY_INSERT [roles] ON;
INSERT INTO [roles] ([id_rol], [nombre_rol])
VALUES (1, 'Administrador'),
(2, 'Veterinario'),
(3, 'Recepcionista');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_rol', N'nombre_rol') AND [object_id] = OBJECT_ID(N'[roles]'))
    SET IDENTITY_INSERT [roles] OFF;
GO

CREATE UNIQUE INDEX [UQ__clientes__AB6E6164286AA167] ON [clientes] ([email]);
GO

CREATE UNIQUE INDEX [UQ__clientes__C2B74E6CB8485CC1] ON [clientes] ([run]);
GO

CREATE INDEX [IX_consultas_id_estado_consulta] ON [consultas] ([id_estado_consulta]);
GO

CREATE INDEX [IX_consultas_id_mascota] ON [consultas] ([id_mascota]);
GO

CREATE INDEX [IX_consultas_id_usuario] ON [consultas] ([id_usuario]);
GO

CREATE INDEX [IX_mascotas_id_cliente] ON [mascotas] ([id_cliente]);
GO

CREATE INDEX [IX_mascotas_id_especie] ON [mascotas] ([id_especie]);
GO

CREATE INDEX [IX_mascotas_id_raza] ON [mascotas] ([id_raza]);
GO

CREATE INDEX [IX_tratamientos_id_consulta] ON [tratamientos] ([id_consulta]);
GO

CREATE INDEX [IX_usuarios_id_estado_usuario] ON [usuarios] ([id_estado_usuario]);
GO

CREATE INDEX [IX_usuarios_id_rol] ON [usuarios] ([id_rol]);
GO

CREATE UNIQUE INDEX [UQ__usuarios__AB6E616446B1A300] ON [usuarios] ([email]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251019124756_SeedRolesAndUserStatuses', N'9.0.10');
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_especie', N'nombre_especie') AND [object_id] = OBJECT_ID(N'[especies]'))
    SET IDENTITY_INSERT [especies] ON;
INSERT INTO [especies] ([id_especie], [nombre_especie])
VALUES (1, 'Perro'),
(2, 'Gato'),
(3, 'Hámster'),
(4, 'Conejo'),
(5, 'Ave');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_especie', N'nombre_especie') AND [object_id] = OBJECT_ID(N'[especies]'))
    SET IDENTITY_INSERT [especies] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_raza', N'nombre_raza') AND [object_id] = OBJECT_ID(N'[razas]'))
    SET IDENTITY_INSERT [razas] ON;
INSERT INTO [razas] ([id_raza], [nombre_raza])
VALUES (1, 'Labrador'),
(2, 'Poodle'),
(3, 'Siamés'),
(4, 'Persa'),
(5, 'Dorado'),
(6, 'Cabeza de León'),
(7, 'Canario');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_raza', N'nombre_raza') AND [object_id] = OBJECT_ID(N'[razas]'))
    SET IDENTITY_INSERT [razas] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251019130119_SeedSpeciesAndBreeds', N'9.0.10');
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251019141809_RemoveRoleSeeding', N'9.0.10');
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[consultas]') AND [c].[name] = N'motivo');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [consultas] DROP CONSTRAINT [' + @var0 + '];');
UPDATE [consultas] SET [motivo] = '' WHERE [motivo] IS NULL;
ALTER TABLE [consultas] ALTER COLUMN [motivo] text NOT NULL;
ALTER TABLE [consultas] ADD DEFAULT '' FOR [motivo];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[consultas]') AND [c].[name] = N'diagnostico');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [consultas] DROP CONSTRAINT [' + @var1 + '];');
UPDATE [consultas] SET [diagnostico] = '' WHERE [diagnostico] IS NULL;
ALTER TABLE [consultas] ALTER COLUMN [diagnostico] text NOT NULL;
ALTER TABLE [consultas] ADD DEFAULT '' FOR [diagnostico];
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_estado_consulta', N'nombre_estado') AND [object_id] = OBJECT_ID(N'[estado_consultas]'))
    SET IDENTITY_INSERT [estado_consultas] ON;
INSERT INTO [estado_consultas] ([id_estado_consulta], [nombre_estado])
VALUES (1, 'Pendiente'),
(2, 'En Progreso'),
(3, 'Finalizada');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id_estado_consulta', N'nombre_estado') AND [object_id] = OBJECT_ID(N'[estado_consultas]'))
    SET IDENTITY_INSERT [estado_consultas] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251019190822_SeedEstadoConsultaData', N'9.0.10');
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[consultas]') AND [c].[name] = N'diagnostico');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [consultas] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [consultas] ALTER COLUMN [diagnostico] text NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251019193237_MakeDiagnosticoNullable', N'9.0.10');
GO

COMMIT;
GO