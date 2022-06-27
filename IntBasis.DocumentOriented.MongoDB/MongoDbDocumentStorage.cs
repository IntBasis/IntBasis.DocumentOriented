namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbDocumentStorage : IDocumentStorage
{
    public Task<T> Retrieve<T>(string id) where T : IDocumentEntity
    {
        throw new NotImplementedException();
    }

    public Task Store(IDocumentEntity entity)
    {
        throw new NotImplementedException();
    }
}
