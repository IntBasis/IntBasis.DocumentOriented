using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbDocumentQuery : IDocumentQuery
{
    private readonly IMongoCollectionService mongoCollectionService;

    public MongoDbDocumentQuery(IMongoCollectionService mongoCollectionService)
    {
        this.mongoCollectionService = mongoCollectionService;
    }

    /// <inheritdoc/>
    public async Task<List<T>> Where<T>(Expression<Func<T, bool>> predicate) where T : IDocumentEntity
    {
        var collection = mongoCollectionService.GetCollection<T>();
        var queryable = collection.AsQueryable<T>();
        return await queryable.Where(predicate).ToListAsync();
    }
}
