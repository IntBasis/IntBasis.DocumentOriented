using System.Linq.Expressions;

namespace IntBasis.DocumentOriented;

/// <summary>
/// Responsible for encapsulating querying the Document Database
/// </summary>
public interface IDocumentQuery
{
    /// <summary>
    /// Return all documents that match the given <paramref name="predicate"/> expression
    /// </summary>
    Task<List<T>> Where<T>(Expression<Func<T, bool>> predicate) where T : IDocumentEntity;
}
