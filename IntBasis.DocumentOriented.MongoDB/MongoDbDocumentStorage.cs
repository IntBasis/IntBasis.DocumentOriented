using Humanizer;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbDocumentStorage : IDocumentStorage
{
    private readonly IMongoDatabaseService mongoDatabaseService;

    public MongoDbDocumentStorage(IMongoDatabaseService mongoDatabaseService)
    {
        this.mongoDatabaseService = mongoDatabaseService;
    }

    /// <inheritdoc/>
    public async Task<T> Retrieve<T>(string id) where T : IDocumentEntity
    {
        var collection = GetCollection<T>();
        var find = collection.Find<T>(document => document.Id == id);
        return await find.FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task Store<T>(T entity) where T : IDocumentEntity
    {
        var collection = GetCollection<T>();
        entity.Id = ObjectId.GenerateNewId().ToString();
        await collection.InsertOneAsync(entity);
    }

    private IMongoCollection<T> GetCollection<T>() where T : IDocumentEntity
    {
        var mongoDatabase = mongoDatabaseService.GetDatabase();
        var collectionName = GetCollectionName<T>();
        return mongoDatabase.GetCollection<T>(collectionName);
    }

    internal static string GetCollectionName<T>()
    {
        // https://stackoverflow.com/a/45335909/483776
        // By convention collection names are plural of the type
        var typeName = typeof(T).Name;
        var plural = typeName.Pluralize();
        // And are lower-cased
        return plural.ToLowerInvariant();
    }
}
