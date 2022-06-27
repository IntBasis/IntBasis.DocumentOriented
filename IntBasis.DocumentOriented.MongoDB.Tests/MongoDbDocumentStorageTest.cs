namespace IntBasis.DocumentOriented.MongoDB.Tests;

class Category : IDocumentEntity
{
    public string? Id { get; set; }
    public string? Name { get; set; }

    public Category(string name)
    {
        Name = name;
    }
}

public class MongoDbDocumentStorageTest
{
    [Fact(DisplayName = "Store")]
    public async Task Storage()
    {
        var subject = new MongoDbDocumentStorage();
        var entity = new Category("test category");
        await subject.Store(entity);
        // entity.Id.Should().NotBeNull();
    }
}
