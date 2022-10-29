namespace IntBasis.DocumentOriented.RavenDB;

public class RavenDbDocumentStorage : IDocumentStorage
{
    private readonly IAsyncDocumentSession documentSession;

    public RavenDbDocumentStorage(IAsyncDocumentSession documentSession)
    {
        this.documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
    }

    // TODO: Sanitize the ID to prevent unexpected behavior: 
    //       https://ravendb.net/docs/article-page/5.3/csharp/server/kb/document-identifier-generation#document-ids---limitations

    /// <inheritdoc/>
    public async Task<T> Retrieve<T>(string id) where T : IDocumentEntity
    {
        var entity = await documentSession.LoadAsync<T>(id);
        DoNotTrackChanges(entity);
        return entity;
    }

    /// <inheritdoc/>
    public async Task Store<T>(T entity) where T : IDocumentEntity
    {
        await documentSession.StoreAsync(entity);
        await documentSession.SaveChangesAsync();
        DoNotTrackChanges(entity);
    }

    /// <summary>
    /// Prevent entity from being "tracked" and changes inadvertently being saved 
    /// when a separate entity is stored.
    /// </summary>
    private void DoNotTrackChanges(IDocumentEntity entity)
    {
        if (entity is null)
            return;
        // We cannot use IgnoreChangesFor(entity)
        // because it doesn't support the "Store Twice" scenario
        // https://ravendb.net/docs/article-page/5.3/csharp/client-api/session/how-to/ignore-entity-changes
        // But, we can use Evict to remove the entity from local session storage
        // so that when it is stored the second time it is like it has never been seen
        // https://ravendb.net/docs/article-page/5.3/csharp/client-api/session/how-to/evict-entity-from-a-session
        documentSession.Advanced.Evict(entity);
    }
}
