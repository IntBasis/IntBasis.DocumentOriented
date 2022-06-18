using System;
using System.Threading.Tasks;

namespace IntBasis.DocumentOriented;

/// <summary>
/// Encapsulates the result of retrieving an entity from <see cref="ISchemaVersionedDocumentStorage"/>
/// so that the caller can both have an immediate response and refreshed response (when needed).
/// </summary>
/// <typeparam name="T">The Entity type</typeparam>
public class SchemaVersionedRetrieval<T> where T : ISchemaVersionedDocumentEntity
{
    /// <summary>
    /// The entity that was retrieved from document storage.
    /// <para/>
    /// It may have been stored with an older Schema Version, so <see cref="IsStale"/> should be checked.
    /// </summary>
    public T Entity { get; init; }

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
    public Task<T> CurrentVersionEntity { get; init; }
}

public interface ISchemaVersionedDocumentStorage
{
    /// <summary>
    /// Store the given entity in configured Document storage.
    /// The <see cref="IDocumentEntity.Id"/> is set to a unique identifier (if it was not already set).
    /// </summary>
    Task Store(ISchemaVersionedDocumentEntity entity);

    /// <summary>
    /// Retrieve the document entity from storage by the given <paramref name="id"/>.
    /// If the stored item was stored with an old schema
    /// (that is the <see cref="ISchemaVersionedDocumentEntity.SchemaVersion"/> is less than version returned from <see cref="ISchemaVersionService"/>)
    /// then <paramref name="refresh"/> is called to get the latest version of the entity.
    /// The latest version of entity will be stored.
    /// </summary>
    Task<SchemaVersionedRetrieval<T>> Retrieve<T>(string id, Func<Task<T>> refresh) where T : ISchemaVersionedDocumentEntity;
}
