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

    [Theory(DisplayName = "Stale Storage"), DictionaryStorage]
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

    [Theory(DisplayName = "Basic Request"), DictionaryStorage]
    public async Task BasicRequest(DictionarySchemaVersionService schemaVersionService,
                                   SchemaVersionedDocumentStorage subject)
    {
        // The stored entity is up-to-date with current schema version
        var id = "11";
        var entity = new VersionedEntity(schemaVersion: 1, id: id, value: "v");
        schemaVersionService.SetSchemaVersion(entity.GetType(), 1);
        await subject.Store(entity);

        var retrieval = await subject.Retrieve<VersionedEntity>(id, () => throw new Exception("Should not refresh because schema is current"));

        retrieval.Entity.Should().Be(entity);
        retrieval.IsStale.Should().BeFalse();
        (await retrieval.CurrentVersionEntity).Should().Be(entity);
    }

    [Theory(DisplayName = "Stale Request"), DictionaryStorage]
    public async Task StaleRequest(DictionarySchemaVersionService schemaVersionService,
                                   DictionaryDocumentStorage documentStorage,
                                   SchemaVersionedDocumentStorage subject)
    {
        // The stored entity has an old schema version
        var id = "11";
        var entity = new VersionedEntity(schemaVersion: 1, id: id, value: "v1");
        await documentStorage.Store(entity);
        schemaVersionService.SetSchemaVersion(entity.GetType(), 2);
        var refreshedEntity = new VersionedEntity(schemaVersion: 2, id: id, value: "v2");

        var retrieval = await subject.Retrieve(id, () => Task.FromResult(refreshedEntity));

        retrieval.Entity.Should().Be(entity);
        retrieval.IsStale.Should().BeTrue();
        (await retrieval.CurrentVersionEntity).Should().Be(refreshedEntity);
        documentStorage.dictionary[id].Should().Be(refreshedEntity);
    }
}
