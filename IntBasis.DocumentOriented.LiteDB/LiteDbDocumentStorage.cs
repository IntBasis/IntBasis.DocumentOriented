using LiteDB;

namespace IntBasis.DocumentOriented.LiteDB;

public class LiteDbDocumentStorage : IDocumentStorage
{
    private readonly ILiteDatabase database;

    public LiteDbDocumentStorage(ILiteDatabase database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    /// <inheritdoc/>
    public Task<T?> Retrieve<T>(string id) where T : IDocumentEntity
    {
        var collection = database.GetCollection<T>();
        return Task.FromResult((T?)collection.FindById(id));
    }

    /// <inheritdoc/>
    public Task Store<T>(T entity) where T : IDocumentEntity
    {
        var collection = database.GetCollection<T>();
        entity.Id ??= ObjectId.NewObjectId().ToString();
        collection.Upsert(entity);
        return Task.CompletedTask;
    }
}
