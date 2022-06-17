namespace IntBasis.DocumentOriented;

/// <summary>
/// Base interface for a Document Entity.
/// Has a string Id which can be used to locate the document
/// </summary>
public interface IDocumentEntity
{
    /// <summary>
    /// Entity ID (unique string)
    /// </summary>
    public string Id { get; set; }
}
