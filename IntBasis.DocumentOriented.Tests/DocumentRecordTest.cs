namespace IntBasis.DocumentOriented.Tests;

public record DocumentRecord(string Id) : IDocumentEntity;

public record SchemaVersionedRecord(string Id) : ISchemaVersionedDocumentEntity
{
    public int SchemaVersion { get; set; }
}

public class DocumentRecordTest
{
    [Fact(DisplayName = "DocumentRecord")]
    public void ConstructDocumentRecord()
    {
        // Verify a `record` can implement our interfaces
        new DocumentRecord("1");
        new SchemaVersionedRecord("2");
    }
}
