-- Script 01: Creación de base de datos TaskyDb
-- Este script crea la base de datos si no existe

USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TaskyDb')
BEGIN
    CREATE DATABASE TaskyDb;
    PRINT 'Base de datos TaskyDb creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos TaskyDb ya existe.';
END
GO

USE TaskyDb;
GO

PRINT 'Base de datos TaskyDb lista para usar.';
GO
