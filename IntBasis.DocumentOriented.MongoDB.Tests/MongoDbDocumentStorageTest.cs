namespace IntBasis.DocumentOriented.MongoDB.Tests;

class Category : IDocumentEntity
{
    public string? Id { get; set; }
}

public class MongoDbDocumentStorageTest
{
    [Fact]
    public void Test1()
    {
        var subject = new MongoDbDocumentStorage();
        var entity = new Category();
        subject.Store(entity);
        // entity.Id.Should().NotBeNull();
    }
}
