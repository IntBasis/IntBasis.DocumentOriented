namespace IntBasis.DocumentOriented.Tests;

public record DocumentRecord(string Id) : IDocumentEntity;

public class DocumentRecordTest
{
    [Fact(DisplayName = "DocumentRecord")]
    public void ConstructDocumentRecord()
    {
        // Verify a `record` can implement our interface
        new DocumentRecord("1");
    }
}