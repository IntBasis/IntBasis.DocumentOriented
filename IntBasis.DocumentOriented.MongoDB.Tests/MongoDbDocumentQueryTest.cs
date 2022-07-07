namespace IntBasis.DocumentOriented.MongoDB.Tests;

public class MongoDbDocumentQueryTest
{
    [Theory(DisplayName = "Empty collection query"), Integration]
    public async Task Empty(MongoDbDocumentQuery subject)
    {
        var result = await subject.Where<Category>(c => c.Name == "none");
        result.Should().BeEmpty();
    }

    [Theory(DisplayName = "Collection one item"), Integration]
    public async Task OneItem(IDocumentStorage documentStorage, MongoDbDocumentQuery subject)
    {
        var name = Guid.NewGuid().ToString();
        var entity = new Category(name);
        await documentStorage.Store(entity);

        var one = await subject.Where<Category>(c => c.Name == name);
        one.Should().HaveCount(1);
        one[0].Should().BeEquivalentTo(entity);

        var zero = await subject.Where<Category>(c => c.Name == name + "_");
        zero.Should().BeEmpty();
    }

    [Theory(DisplayName = "Collection two items"), Integration]
    public async Task TwoItems(IDocumentStorage documentStorage, MongoDbDocumentQuery subject)
    {
        var prefix = Guid.NewGuid().ToString();
        await documentStorage.Store(new Category(prefix + "1"));
        await documentStorage.Store(new Category(prefix + "2"));

        var result = await subject.Where<Category>(c => c.Name != null && c.Name.StartsWith(prefix));
        result.Should().HaveCount(2);
    }
}
