#if MONGO_DB
namespace IntBasis.DocumentOriented.MongoDB.Tests;
#elif RAVEN_DB
namespace IntBasis.DocumentOriented.RavenDB.Tests;
#elif LITE_DB
namespace IntBasis.DocumentOriented.LiteDB.Tests;
#endif

public class DocumentQueryTest
{
    [Theory(DisplayName = "Empty collection query"), Integration]
    public async Task Empty(IDocumentQuery subject)
    {
        var result = await subject.Where<Category>(c => c.Name == "none");
        result.Should().BeEmpty();
    }

    [Theory(DisplayName = "Collection one item"), Integration]
    public async Task OneItem(IDocumentStorage documentStorage, IDocumentQuery subject)
    {
        var name = Guid.NewGuid().ToString();
        var entity = new Category(name);
        await documentStorage.Store(entity);

        // HACK: Wait for index to be updated (query after write)
        await Task.Delay(100);

        var one = await subject.Where<Category>(c => c.Name == name);
        one.Should().HaveCount(1);
        one[0].Should().BeEquivalentTo(entity);

        var wrongName = name + "_";
        var zero = await subject.Where<Category>(c => c.Name == wrongName);
        zero.Should().BeEmpty();
    }

    [Theory(DisplayName = "Collection two items"), Integration]
    public async Task TwoItems(IDocumentStorage documentStorage, IDocumentQuery subject)
    {
        var prefix = Guid.NewGuid().ToString();
        await documentStorage.Store(new Category(prefix + "1"));
        await documentStorage.Store(new Category(prefix + "2"));

        // HACK: Wait for index to be updated (query after write)
        await Task.Delay(100);

        var result = await subject.Where<Category>(c => c.Name != null && c.Name.StartsWith(prefix));
        result.Should().HaveCount(2);
    }
}
