using System;
using System.Threading.Tasks;

namespace IntBasis.DocumentOriented;

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
