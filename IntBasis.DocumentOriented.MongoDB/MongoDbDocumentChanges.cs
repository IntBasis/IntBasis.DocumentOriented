using System.Reactive.Concurrency;
using System.Reactive.Linq;
using static System.Reactive.Linq.Observable;

namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbDocumentChanges : IDocumentChanges
{
    private readonly IMongoCollectionService mongoCollectionService;

    public MongoDbDocumentChanges(IMongoCollectionService mongoCollectionService)
    {
        this.mongoCollectionService = mongoCollectionService ?? throw new ArgumentNullException(nameof(mongoCollectionService));
    }

    /// <inheritdoc/>
    public IDisposable Subscribe<T>(Func<DocumentChangeInfo, Task> observer) where T : class, IDocumentEntity
    {
        // https://mongodb.github.io/mongo-csharp-driver/2.9/reference/driver/change_streams/
        var collection = mongoCollectionService.GetCollection<T>();
        var cursor = collection.Watch();
        // Use a Scheduler so this is not blocked by the subscription
        var observable = cursor.ToEnumerable()
                               .ToObservable(TaskPoolScheduler.Default);
        var subscription = observable.Select(document => FromAsync(() => observer(Convert(document))))
                                     .Concat()
                                     .Subscribe();
        return new MultiDisposable(subscription, cursor);
    }

    private DocumentChangeInfo Convert<T>(ChangeStreamDocument<T> changeStreamDocument) where T : IDocumentEntity
    {
        var documentId = changeStreamDocument.FullDocument.Id
                         ?? throw new Exception("A change occurred but 'ChangeStreamDocument.FullDocument.Id' was null.");
        return new DocumentChangeInfo(documentId);
    }
}
