namespace Tasky.Domain.Exceptions;

/// <summary>
/// Excepción personalizada para errores de lógica de negocio en el dominio
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
