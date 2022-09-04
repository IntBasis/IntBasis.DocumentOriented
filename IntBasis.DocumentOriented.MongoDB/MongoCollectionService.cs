using Humanizer;

namespace IntBasis.DocumentOriented.MongoDB;

public interface IMongoCollectionService
{
    /// <summary>
    /// Returns the Mongo DB Collection for the given Type
    /// using a naming convention derived from the pluralized type name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IMongoCollection<T> GetCollection<T>() where T : IDocumentEntity;
}

internal class MongoCollectionService : IMongoCollectionService
{
    private readonly IMongoDatabaseService mongoDatabaseService;

    public MongoCollectionService(IMongoDatabaseService mongoDatabaseService)
    {
        this.mongoDatabaseService = mongoDatabaseService;
    }

    public IMongoCollection<T> GetCollection<T>() where T : IDocumentEntity
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
