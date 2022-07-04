namespace IntBasis.DocumentOriented;

/// <summary>
/// Base interface for a Document Entity.
/// Has a string Id which can be used to locate the document
/// </summary>
public interface IDocumentEntity
{
    /// <summary>
    /// Entity ID (unique string)
    /// <para/>
    /// The ID may be null before it has been stored. After the entity has been stored the ID must not be null.
    /// </summary>
    public string? Id { get; set; }
}
