using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Tasky.Infrastructure.Persistence;

/// <summary>
/// Factory para crear el DbContext en tiempo de diseño (migraciones)
/// </summary>
public class TaskyDbContextFactory : IDesignTimeDbContextFactory<TaskyDbContext>
{
    public TaskyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaskyDbContext>();

        // Configuración por defecto para migraciones
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=TaskyDb;User Id=sa;Password=Tasky@2026!;TrustServerCertificate=True;");

        return new TaskyDbContext(optionsBuilder.Options);
    }
}
