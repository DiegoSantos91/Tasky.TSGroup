namespace Tasky.Domain.Common;

/// <summary>
/// Clase base para todas las entidades del dominio
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Fecha de creación de la entidad
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Fecha de última actualización de la entidad
    /// </summary>
    public DateTime UpdatedAt { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
