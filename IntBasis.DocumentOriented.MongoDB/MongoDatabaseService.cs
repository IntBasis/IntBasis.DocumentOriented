using MongoDB.Driver;

namespace IntBasis.DocumentOriented.MongoDB;

internal class MongoDatabaseService : IMongoDatabaseService
{
    MongoDbConfiguration config;

    public MongoDatabaseService(MongoDbConfiguration config)
    {
        this.config = config;
    }

    /// <inheritdoc/>
    public IMongoDatabase GetDatabase()
    {
        var client = new MongoClient(config.ConnectionString);
        return client.GetDatabase(config.DatabaseName);
    }
}
