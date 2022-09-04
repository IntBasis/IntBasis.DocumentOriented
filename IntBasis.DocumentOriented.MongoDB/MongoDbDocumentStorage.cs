using MongoDB.Bson;

namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbDocumentStorage : IDocumentStorage
{
    private readonly IMongoCollectionService mongoCollectionService;

    public MongoDbDocumentStorage(IMongoCollectionService mongoCollectionService)
    {
        this.mongoCollectionService = mongoCollectionService;
    }

    /// <inheritdoc/>
    public async Task<T?> Retrieve<T>(string id) where T : IDocumentEntity
    {
        var collection = mongoCollectionService.GetCollection<T>();
        var find = collection.Find<T>(document => document.Id == id);
        return await find.FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task Store<T>(T entity) where T : IDocumentEntity
    {
        var collection = mongoCollectionService.GetCollection<T>();
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
}
