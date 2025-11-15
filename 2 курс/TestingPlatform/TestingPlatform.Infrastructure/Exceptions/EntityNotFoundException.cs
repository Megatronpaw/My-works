namespace TestingPlatform.Infrastructure.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException() : base() { }

    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }

    public EntityNotFoundException(string entityName, int entityId)
        : base($"{entityName} с ID {entityId} не найден.") { }
}