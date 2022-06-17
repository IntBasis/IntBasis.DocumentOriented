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

    public SchemaVersionedRetrieval<T> Retrieve<T>(string id, Func<Task<T>> refresh) where T : ISchemaVersionedDocumentEntity
    {
        throw new NotImplementedException();
    }

    public async Task Store(ISchemaVersionedDocumentEntity entity)
    {
        await documentStorage.Store(entity);
    }
}