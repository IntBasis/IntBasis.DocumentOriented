using System;
using System.Threading.Tasks;

namespace IntBasis.DocumentOriented;

public class SchemaVersionedDocumentStorage : ISchemaVersionedDocumentStorage
{
    public IDocumentStorage documentStorage;
    public ISchemaVersionService schemaVersionService;

    public SchemaVersionedDocumentStorage(IDocumentStorage documentStorage, ISchemaVersionService schemaVersionService)
    {
        this.documentStorage = documentStorage ?? throw new ArgumentNullException(nameof(documentStorage));
        this.schemaVersionService = schemaVersionService ?? throw new ArgumentNullException(nameof(schemaVersionService));
    }

    /// <inheritdoc/>
    public async Task<SchemaVersionedRetrieval<T>> Retrieve<T>(string id, Func<Task<T>> refresh) where T : ISchemaVersionedDocumentEntity
    {
        var entity = await documentStorage.Retrieve<T>(id);
        return new SchemaVersionedRetrieval<T>
        {
            Entity = entity,
            IsStale = false,
            CurrentVersion = Task.FromResult(entity)
        };
    }

    /// <inheritdoc/>
    public async Task Store(ISchemaVersionedDocumentEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        var type = entity.GetType();
        var currentVersion = schemaVersionService.GetCurrentSchemaVersion(type);
        if (entity.SchemaVersion < currentVersion)
        {
            string message = $"The entity's schema version ({entity.SchemaVersion}) " +
                $"is less than the current schema version ({currentVersion}) for Type '{type}'.";
            throw new InvalidOperationException(message);
        }
        await documentStorage.Store(entity);
    }
}