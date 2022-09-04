using System.Linq.Expressions;

namespace IntBasis.DocumentOriented.RavenDB;

class RavenDbDocumentQuery : IDocumentQuery
{
    private readonly IAsyncDocumentSession documentSession;

    public RavenDbDocumentQuery(IAsyncDocumentSession documentSession)
    {
        this.documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
    }

    /// <inheritdoc/>
    public Task<List<T>> Where<T>(Expression<Func<T, bool>> predicate) where T : IDocumentEntity
    {
        return documentSession.Query<T>()
                              .Where(predicate)
                              .ToListAsync();
    }
}
