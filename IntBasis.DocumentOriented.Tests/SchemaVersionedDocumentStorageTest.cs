namespace IntBasis.DocumentOriented.Tests;

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
    [Theory(DisplayName = "Basic Storage"), DictionaryStorage]
    public async Task BasicStorage(DictionarySchemaVersionService schemaVersionService,
                                   DictionaryDocumentStorage documentStorage,
                                   SchemaVersionedDocumentStorage subject)
    {
        var entity = new VersionedEntity(schemaVersion: 1, id: "10", value: "v");
        schemaVersionService.SetSchemaVersion(entity.GetType(), 1);

        await subject.Store(entity);

        documentStorage.dictionary["10"].Should().Be(entity);
    }

    [Theory(DisplayName = "Old Version Storage"), DictionaryStorage]
    public async Task OldVersion(DictionarySchemaVersionService schemaVersionService,
                                 SchemaVersionedDocumentStorage subject)
    {
        var entity = new VersionedEntity(schemaVersion: 1, id: "10", value: "v");
        schemaVersionService.SetSchemaVersion(entity.GetType(), 2);

        await subject.Invoking(s => s.Store(entity))
                     .Should()
                     .ThrowAsync<InvalidOperationException>()
                     .WithMessage("*schema version*");
    }
}
