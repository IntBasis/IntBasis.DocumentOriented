namespace IntBasis.DocumentOriented.Tests;

public record DocumentRecord(string Id) : IDocumentEntity;

public record SchemaVersionedRecord(string Id) : ISchemaVersionedDocumentEntity
{
    public int SchemaVersion { get; set; }

    public SchemaVersionedRecord(string id, int schemaVersion) : this(id)
    {
        SchemaVersion = schemaVersion;
    }
}

public class DocumentRecordTest
{
    [Fact(DisplayName = "DocumentRecord")]
    public void ConstructDocumentRecord()
    {
        // Verify a `record` can implement our interfaces
        var documentRecord = new DocumentRecord("1");
        documentRecord.Id.Should().Be("1");

        var schemaVersionedRecord = new SchemaVersionedRecord("2", 42);
        schemaVersionedRecord.Id.Should().Be("2");
        schemaVersionedRecord.SchemaVersion.Should().Be(42);
    }
}
