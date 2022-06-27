using MongoDB.Driver;

namespace IntBasis.DocumentOriented.MongoDB;

public interface IMongoDatabaseService
{
    /// <summary>
    /// Return connection to configured MongoDB database    
    /// </summary>
    IMongoDatabase GetDatabase();
}
