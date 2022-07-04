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
    public async Task<T?> Retrieve<T>(string id) where T : IDocumentEntity
    {
        var collection = GetCollection<T>();
        var find = collection.Find<T>(document => document.Id == id);
        return await find.FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task Store<T>(T entity) where T : IDocumentEntity
    {
        var collection = GetCollection<T>();
        if (entity.Id is null)
            entity.Id = ObjectId.GenerateNewId().ToString();
        await Save(collection, entity);
    }

    /// Inserts or Updates the given entity in the given collection
    public async Task<ReplaceOneResult> Save<T>(IMongoCollection<T> collection, T entity) where T : IDocumentEntity
    {
        // Save() used to be a part of the .NET driver, but was removed
        // https://stackoverflow.com/a/30385288/483776
        var replaceOneResult = await collection.ReplaceOneAsync(doc => doc.Id == entity.Id,
                                                                entity,
                                                                new ReplaceOptions { IsUpsert = true });
        // You can look at ReplaceOneResult.MatchedCount to see whether it was an insert or update.
        return replaceOneResult;
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
