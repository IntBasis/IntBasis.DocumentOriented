namespace IntBasis.DocumentOriented.Tests;

internal class DictionaryDocumentStorage : IDocumentStorage
{
    internal readonly Dictionary<string, IDocumentEntity> dictionary = new();
    private int nextId = 999;

    public Task<T> Retrieve<T>(string id) where T : IDocumentEntity
    {
        var entity = (T) dictionary[id];
        return Task.FromResult(entity);
    }

    public Task Store(IDocumentEntity entity)
    {
        // Use given ID if it has one; otherwise generate one.
        var id = entity.Id ?? GetNextId();
        entity.Id = id;
        dictionary[id] = entity;
        return Task.CompletedTask;
    }

    private string GetNextId()
    {
        ++nextId;
        return nextId.ToString();
    }
}

internal class DictionarySchemaVersionService : ISchemaVersionService
{
    private readonly Dictionary<Type, int> dictionary = new();

    internal void SetSchemaVersion(Type type, int schemaVersion)
    {
        dictionary[type] = schemaVersion;
    }

    public int GetCurrentSchemaVersion(Type type)
    {
        return dictionary[type];
    }
}

internal class VersionedEntity : ISchemaVersionedDocumentEntity
{
    public int SchemaVersion { get; set; }
    public string? Id { get; set; }
    public string? Value { get; set; }

    public VersionedEntity(int schemaVersion, string? id, string? value)
    {
        SchemaVersion = schemaVersion;
        Id = id;
        Value = value;
    }
}

public class SchemaVersionedDocumentStorageTest
{
    [Fact(DisplayName = "Basic Storage")]
    public async Task BasicStorage()
    {
        var schemaVersionService = new DictionarySchemaVersionService();
        var documentStorage = new DictionaryDocumentStorage();
        var subject = new SchemaVersionedDocumentStorage(documentStorage, schemaVersionService);
        var entity = new VersionedEntity(schemaVersion: 1, id: "10", value: "v");
        schemaVersionService.SetSchemaVersion(entity.GetType(), 1);

        await subject.Store(entity);

        documentStorage.dictionary["10"].Should().Be(entity);
    }
}