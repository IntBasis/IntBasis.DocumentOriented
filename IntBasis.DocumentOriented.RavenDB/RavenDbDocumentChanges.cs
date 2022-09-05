using Raven.Client.Documents.Changes;
using static System.Reactive.Linq.Observable;

namespace IntBasis.DocumentOriented.RavenDB;

public class RavenDbDocumentChanges : IDocumentChanges
{
    private readonly IDocumentStore documentStore;

    public RavenDbDocumentChanges(IDocumentStore documentStore)
    {
        this.documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
    }

    /// <inheritdoc/>
    public IDisposable Subscribe<T>(Func<DocumentChangeInfo, Task> observer) where T : IDocumentEntity
    {
        // [Changes API: How to Subscribe to Document Changes](https://ravendb.net/docs/article-page/5.3/csharp/client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
        // [Async / Await with IObserver](https://github.com/dotnet/reactive/issues/459)
        var observable = documentStore.Changes()
                                      .ForDocumentsInCollection<T>();
        return observable.Select(document => FromAsync(() => observer(Convert(document))))
                         .Concat()
                         .Subscribe();
    }

    private DocumentChangeInfo Convert(DocumentChange documentChange)
    {
        return new DocumentChangeInfo(documentChange.Id);
    }
}
