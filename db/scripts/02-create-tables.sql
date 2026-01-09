-- Script 02: Creación de tablas
-- Este script crea las tablas principales de la aplicación
-- Nota: Las tablas también serán gestionadas por Entity Framework Migrations

USE TaskyDb;
GO

-- Tabla TodoTasks
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TodoTasks')
BEGIN
    CREATE TABLE TodoTasks (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        IsCompleted BIT NOT NULL DEFAULT 0,
        DueDate DATETIME2(7) NULL,
        CompletedAt DATETIME2(7) NULL,
        CreatedAt DATETIME2(7) NOT NULL,
        UpdatedAt DATETIME2(7) NOT NULL
    );

    -- Índices para mejorar el rendimiento
    CREATE INDEX IX_TodoTasks_IsCompleted ON TodoTasks(IsCompleted);
    CREATE INDEX IX_TodoTasks_DueDate ON TodoTasks(DueDate);
    CREATE INDEX IX_TodoTasks_CreatedAt ON TodoTasks(CreatedAt);

    PRINT 'Tabla TodoTasks creada exitosamente con índices.';
END
ELSE
BEGIN
    PRINT 'La tabla TodoTasks ya existe.';
END
GO
