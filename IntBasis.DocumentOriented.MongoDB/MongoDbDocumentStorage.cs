using MongoDB.Driver;

namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbDocumentStorage : IDocumentStorage
{
    public Task<T> Retrieve<T>(string id) where T : IDocumentEntity
    {
        throw new NotImplementedException();
    }

    public Task Store(IDocumentEntity entity)
    {
        var connectionString = "mongodb://localhost:27017";
        var client = new MongoClient(connectionString);
        var databaseName = "test";
        var database = client.GetDatabase(databaseName);
        // database.GetCollection<T>
        return Task.CompletedTask;
    }
}
