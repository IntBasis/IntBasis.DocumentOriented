using Raven.Client.Documents.Session;
using System;

namespace IntBasis.DocumentOriented.RavenDB;

public class RavenDbDocumentStorage : IDocumentStorage
{
    private readonly IDocumentSession documentSession;

    public RavenDbDocumentStorage(IDocumentSession documentSession)
    {
        this.documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
    }

    /// <inheritdoc/>
    public T Retrieve<T>(string id) where T : IDocumentEntity
    {
        var entity = documentSession.Load<T>(id);
        DoNotTrackChanges(entity);
        return entity;
    }

    /// <inheritdoc/>
    public void Store(IDocumentEntity entity)
    {
        documentSession.Store(entity);
        documentSession.SaveChanges();
        DoNotTrackChanges(entity);
    }

    /// <summary>
    /// Prevent entity from being "tracked" and changes inadvertantly being saved 
    /// when a separate entity is stored.
    /// </summary>
    private void DoNotTrackChanges(IDocumentEntity entity)
    {
        // We cannot use IgnoreChangesFor(entity)
        // because it doesn't support the "Store Twice" scenario
        // https://ravendb.net/docs/article-page/5.3/csharp/client-api/session/how-to/ignore-entity-changes
        // But, we can use Evict to remove the entity from local session storage
        // so that when it is stored the second time it is like it has never been seen
        // https://ravendb.net/docs/article-page/5.3/csharp/client-api/session/how-to/evict-entity-from-a-session
        documentSession.Advanced.Evict(entity);
    }
}
