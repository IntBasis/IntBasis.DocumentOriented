namespace IntBasis.DocumentOriented;

/// <summary>
/// Provides information on changes made to a Document collection
/// </summary>
public class DocumentChangeInfo
{
    /// <summary>
    /// Corresponds to the <see cref="IDocumentEntity.Id"/> of the Document Entity that was inserted or changed.
    /// </summary>
    public string DocumentId { get; }

    public DocumentChangeInfo(string documentId)
    {
        DocumentId = documentId ?? throw new ArgumentNullException(nameof(documentId));
    }
}
