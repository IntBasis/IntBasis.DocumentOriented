using System.Threading.Tasks;

namespace IntBasis.DocumentOriented;

/// <summary>
/// Encapsulates the simple storage and retrieval of individual document entities by Id.
/// </summary>
public interface IDocumentStorage
{
    /// <summary>
    /// Store the given entity in configured Document storage.
    /// The <see cref="IDocumentEntity.Id"/> is set to a unique identifier (if it was not already set).
    /// </summary>
    Task Store<T>(T entity) where T : IDocumentEntity;

    /// <summary>
    /// Retrieve the document entity from storage by the given <paramref name="id"/>.
    /// <para/>
    /// Returns null if the entity is not found.
    /// </summary>
    Task<T> Retrieve<T>(string id) where T : IDocumentEntity;
}
