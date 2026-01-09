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
CREATE TABLE [TodoTasks] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(1000) NOT NULL,
    [IsCompleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DueDate] datetime2 NULL,
    [CompletedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_TodoTasks] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_TodoTasks_CreatedAt] ON [TodoTasks] ([CreatedAt]);

CREATE INDEX [IX_TodoTasks_DueDate] ON [TodoTasks] ([DueDate]);

CREATE INDEX [IX_TodoTasks_IsCompleted] ON [TodoTasks] ([IsCompleted]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260108065501_InitialCreate', N'9.0.0');

COMMIT;
GO

