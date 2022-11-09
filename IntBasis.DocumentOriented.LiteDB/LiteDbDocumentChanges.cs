using LiteDB.Realtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using static System.Reactive.Linq.Observable;

namespace IntBasis.DocumentOriented.LiteDB;

public class LiteDbDocumentChanges : IDocumentChanges
{
    private readonly RealtimeLiteDatabase database;

    public LiteDbDocumentChanges(RealtimeLiteDatabase database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public IDisposable Subscribe<T>(Func<DocumentChangeInfo, Task> observer) where T : class, IDocumentEntity
    {
        // HACK: Get the collection name which is required by Realtime.Collection
        var collectionName = database.GetCollection<T>().Name;
        // Un-buffer the Observable<List<T>> to get Observable<T>
        var observable = database.Realtime.Collection<T>(collectionName)
                                          .Select(documents => new ObservableCollection<T>(documents))
                                          .SelectMany(x => x);
        return observable.Select(document => FromAsync(() => observer(Convert(document))))
                         .Concat()
                         .Subscribe();
    }

    private DocumentChangeInfo Convert<T>(T document) where T : class, IDocumentEntity
    {
        if (document.Id is null)
            throw new InvalidOperationException("The RealtimeLiteDatabase raised a notification for a document with a null Id.");
        return new DocumentChangeInfo(document.Id);
    }
}
