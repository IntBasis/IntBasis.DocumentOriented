using MongoDB.Driver;

namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbDocumentStorage : IDocumentStorage
{
    /// <inheritdoc/>
    public Task<T> Retrieve<T>(string id) where T : IDocumentEntity
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task Store<T>(T entity) where T : IDocumentEntity
    {
        var connectionString = "mongodb://localhost:27017";
        var client = new MongoClient(connectionString);
        var databaseName = "test";
        var database = client.GetDatabase(databaseName);
        var collectionName = "entities";
        var collection = database.GetCollection<T>(collectionName);
        await collection.InsertOneAsync(entity);
    }
}
