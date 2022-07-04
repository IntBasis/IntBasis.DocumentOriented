using System.Threading.Tasks;

namespace IntBasis.DocumentOriented;

/// <summary>
/// Encapsulates the result of retrieving an entity from <see cref="ISchemaVersionedDocumentStorage"/>
/// so that the caller can both have an immediate response and refreshed response (when needed).
/// </summary>
/// <typeparam name="TEntity">The Entity type</typeparam>
public class SchemaVersionedRetrieval<TEntity> where TEntity : ISchemaVersionedDocumentEntity
{
    /// <summary>
    /// The entity that was retrieved from document storage.
    /// <para/>
    /// May be null if the entity was not found in storage.
    /// <para/>
    /// It may have been stored with an older Schema Version, so <see cref="IsStale"/> should be checked.
    /// </summary>
    public TEntity? Entity { get; init; }

    /// <summary>
    /// True if the retrieved <see cref="Entity"/> was stored with an older Schema Version.
    /// </summary>
    public bool IsStale { get; init; }

    /// <summary>
    /// Returns the Entity with the Current Schema Version.
    /// <para/>
    /// For stale entity this Task will return the result of the refresh operation (which should have current schema version).
    /// This may be a long-running operation.
    /// <para/>
    /// For non-stale entities it will return the same object as <see cref="Entity"/>.
    /// </summary>
    public Task<TEntity> CurrentVersionEntity { get; init; }

    public SchemaVersionedRetrieval(TEntity? entity, bool isStale, Task<TEntity> currentVersionEntity)
    {
        Entity = entity;
        IsStale = isStale;
        CurrentVersionEntity = currentVersionEntity;
    }
}

public static class SchemaVersionedRetrieval
{
    /// <summary>
    /// Creates a new <see cref="SchemaVersionedRetrieval"/> for an entity that is current (not stale)
    /// </summary>
    public static SchemaVersionedRetrieval<T> Current<T>(T entity) where T : ISchemaVersionedDocumentEntity
    {
        return new SchemaVersionedRetrieval<T>(entity, isStale: false, currentVersionEntity: Task.FromResult(entity));
    }
}
