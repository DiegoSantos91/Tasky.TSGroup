-- Script 03: Datos de prueba
-- Este script inserta datos de ejemplo para testing

USE TaskyDb;
GO

-- Limpiar datos existentes de prueba (opcional, comentar si no se desea)
-- DELETE FROM TodoTasks WHERE Title LIKE '%[DEMO]%';

-- Insertar tareas de ejemplo
DECLARE @Now DATETIME2(7) = GETUTCDATE();

-- Tarea completada
IF NOT EXISTS (SELECT 1 FROM TodoTasks WHERE Title = '[DEMO] Tarea completada de ejemplo')
BEGIN
    INSERT INTO TodoTasks (Id, Title, Description, IsCompleted, DueDate, CompletedAt, CreatedAt, UpdatedAt)
    VALUES (
        NEWID(),
        '[DEMO] Tarea completada de ejemplo',
        'Esta es una tarea que ya fue completada',
        1,
        DATEADD(DAY, -2, @Now),
        DATEADD(DAY, -1, @Now),
        DATEADD(DAY, -3, @Now),
        DATEADD(DAY, -1, @Now)
    );
    PRINT 'Tarea completada insertada.';
END

-- Tarea pendiente con fecha de vencimiento futura
IF NOT EXISTS (SELECT 1 FROM TodoTasks WHERE Title = '[DEMO] Implementar nueva funcionalidad')
BEGIN
    INSERT INTO TodoTasks (Id, Title, Description, IsCompleted, DueDate, CompletedAt, CreatedAt, UpdatedAt)
    VALUES (
        NEWID(),
        '[DEMO] Implementar nueva funcionalidad',
        'Desarrollar la nueva funcionalidad solicitada por el cliente. Incluir pruebas unitarias y documentación.',
        0,
        DATEADD(DAY, 7, @Now),
        NULL,
        @Now,
        @Now
    );
    PRINT 'Tarea pendiente con vencimiento futuro insertada.';
END

-- Tarea vencida
IF NOT EXISTS (SELECT 1 FROM TodoTasks WHERE Title = '[DEMO] Tarea vencida')
BEGIN
    INSERT INTO TodoTasks (Id, Title, Description, IsCompleted, DueDate, CompletedAt, CreatedAt, UpdatedAt)
    VALUES (
        NEWID(),
        '[DEMO] Tarea vencida',
        'Esta tarea debía completarse ayer',
        0,
        DATEADD(DAY, -1, @Now),
        NULL,
        DATEADD(DAY, -5, @Now),
        DATEADD(DAY, -5, @Now)
    );
    PRINT 'Tarea vencida insertada.';
END

-- Tarea sin fecha de vencimiento
IF NOT EXISTS (SELECT 1 FROM TodoTasks WHERE Title = '[DEMO] Revisar código')
BEGIN
    INSERT INTO TodoTasks (Id, Title, Description, IsCompleted, DueDate, CompletedAt, CreatedAt, UpdatedAt)
    VALUES (
        NEWID(),
        '[DEMO] Revisar código',
        'Hacer code review del pull request #123',
        0,
        NULL,
        NULL,
        @Now,
        @Now
    );
    PRINT 'Tarea sin vencimiento insertada.';
END

-- Tarea urgente
IF NOT EXISTS (SELECT 1 FROM TodoTasks WHERE Title = '[DEMO] Fix bug crítico')
BEGIN
    INSERT INTO TodoTasks (Id, Title, Description, IsCompleted, DueDate, CompletedAt, CreatedAt, UpdatedAt)
    VALUES (
        NEWID(),
        '[DEMO] Fix bug crítico',
        'Corregir el bug en producción reportado por el cliente',
        0,
        DATEADD(HOUR, 4, @Now),
        NULL,
        DATEADD(HOUR, -1, @Now),
        DATEADD(HOUR, -1, @Now)
    );
    PRINT 'Tarea urgente insertada.';
END

PRINT '';
PRINT '==================================================';
PRINT 'Datos de prueba insertados exitosamente.';
PRINT 'Total de tareas en la base de datos:';
SELECT COUNT(*) AS TotalTareas FROM TodoTasks;
PRINT '==================================================';
GO
