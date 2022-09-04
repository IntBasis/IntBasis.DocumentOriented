using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbDocumentChanges : IDocumentChanges
{
    private readonly IMongoCollectionService mongoCollectionService;

    public MongoDbDocumentChanges(IMongoCollectionService mongoCollectionService)
    {
        this.mongoCollectionService = mongoCollectionService ?? throw new ArgumentNullException(nameof(mongoCollectionService));
    }

    public IDisposable Subscribe<T>(Func<Task> observer) where T : IDocumentEntity
    {
        // https://mongodb.github.io/mongo-csharp-driver/2.9/reference/driver/change_streams/
        var collection = mongoCollectionService.GetCollection<T>();
        var cursor = collection.Watch();
        // Use a Scheduler so this is not blocked by the subscription
        var observable = cursor.ToEnumerable()
                               .ToObservable(TaskPoolScheduler.Default);
        var subscription = observable.Select(document => Observable.FromAsync(observer))
                                     .Concat()
                                     .Subscribe();
        return new MultiDisposable(subscription, cursor);
    }
}
