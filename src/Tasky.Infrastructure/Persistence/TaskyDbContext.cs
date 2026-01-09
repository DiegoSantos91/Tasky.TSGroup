using Microsoft.EntityFrameworkCore;
using Tasky.Domain.Entities;
using System.Reflection;

namespace Tasky.Infrastructure.Persistence;

/// <summary>
/// Contexto de base de datos para la aplicación Tasky
/// </summary>
public class TaskyDbContext : DbContext
{
    public TaskyDbContext(DbContextOptions<TaskyDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas las configuraciones del ensamblado actual
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Intercepta el guardado para actualizar automáticamente las propiedades de auditoría
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Actualizar timestamps de las entidades modificadas
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Domain.Common.BaseEntity &&
                       (e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((Domain.Common.BaseEntity)entityEntry.Entity).UpdateTimestamp();
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
