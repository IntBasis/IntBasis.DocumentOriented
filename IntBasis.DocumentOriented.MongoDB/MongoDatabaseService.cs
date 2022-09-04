namespace IntBasis.DocumentOriented.MongoDB;

internal class MongoDatabaseService : IMongoDatabaseService
{
    private readonly MongoDbConfiguration config;

    public MongoDatabaseService(MongoDbConfiguration config)
    {
        this.config = config;
    }

    /// <inheritdoc/>
    public IMongoDatabase GetDatabase()
    {
        var client = new MongoClient(config.ConnectionString);
        // TODO: Sanitize DB name for Mongo requirements
        //       "Database names must be non-empty and not contain '.' or the null character."
        return client.GetDatabase(config.DatabaseName);
    }
}
