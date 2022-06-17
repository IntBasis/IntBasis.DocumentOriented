using System;
using System.Threading.Tasks;

namespace IntBasis.DocumentOriented;

public class SchemaVersionedRetrieval<T> where T : ISchemaVersionedDocumentEntity
{
    /// <summary>
    /// The entity that was retrieved from document storage
    /// </summary>
    public T Entity { get; init; }

    /// <summary>
    /// True if the retrieved <see cref="Entity"/> was stored with an older Schema Version.
    /// </summary>
    public bool IsStale { get; init; }

    /// <summary>
    /// For stale entity this Task will return the result of the refresh operation (which should have current schema version).
    /// For non-stale entities it will return <see cref="Entity"/>.
    /// </summary>
    public Task<T> CurrentVersion { get; init; }
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
    SchemaVersionedRetrieval<T> Retrieve<T>(string id, Func<Task<T>> refresh) where T : ISchemaVersionedDocumentEntity;
}
