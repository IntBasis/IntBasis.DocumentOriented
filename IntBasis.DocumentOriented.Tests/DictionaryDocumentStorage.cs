namespace IntBasis.DocumentOriented.Tests;

public class DictionaryDocumentStorage : IDocumentStorage
{
    internal readonly Dictionary<string, IDocumentEntity> dictionary = new();
    private int nextId = 999;

    public Task<T> Retrieve<T>(string id) where T : IDocumentEntity
    {
        if (!dictionary.ContainsKey(id))
            return Task.FromResult(default(T));
        var entity = (T)dictionary[id];
        return Task.FromResult(entity);
    }

    public Task Store<T>(T entity) where T : IDocumentEntity
    {
        // Use given ID if it has one; otherwise generate one.
        var id = entity.Id ?? GetNextId();
        entity.Id = id;
        dictionary[id] = entity;
        return Task.CompletedTask;
    }

    private string GetNextId()
    {
        ++nextId;
        return nextId.ToString();
    }
}
