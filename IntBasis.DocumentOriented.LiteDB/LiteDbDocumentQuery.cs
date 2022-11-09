using LiteDB;
using System.Linq.Expressions;

namespace IntBasis.DocumentOriented.LiteDB;

public class LiteDbDocumentQuery : IDocumentQuery
{
    private readonly ILiteDatabase database;

    public LiteDbDocumentQuery(ILiteDatabase database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public Task<List<T>> Where<T>(Expression<Func<T, bool>> predicate) where T : IDocumentEntity
    {
        var collection = database.GetCollection<T>();
        var result = collection.Find(predicate)
                               .ToList();
        return Task.FromResult(result);
    }
}
