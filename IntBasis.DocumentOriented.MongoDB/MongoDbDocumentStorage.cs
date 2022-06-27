using MongoDB.Bson;
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
        IMongoDatabase database = OpenTestDatabase();
        var collectionName = "entities";
        var collection = database.GetCollection<T>(collectionName);
        entity.Id = ObjectId.GenerateNewId().ToString();
        await collection.InsertOneAsync(entity);
    }

    internal static IMongoDatabase OpenTestDatabase()
    {
        var connectionString = "mongodb://localhost:27017";
        var client = new MongoClient(connectionString);
        var databaseName = "test";
        var database = client.GetDatabase(databaseName);
        return database;
    }
}
