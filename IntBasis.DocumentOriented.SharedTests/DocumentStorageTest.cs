using System.Dynamic;

#if MONGO_DB
namespace IntBasis.DocumentOriented.MongoDB.Tests;
#else
namespace IntBasis.DocumentOriented.RavenDB.Tests;
#endif

public class Category : IDocumentEntity
{
    public string? Id { get; set; }
    public string? Name { get; set; }

    public Category(string name)
    {
        Name = name;
    }

    internal Category(string? id, string name) : this(name)
    {
        Id = id;
    }
}

public class DocumentStorageTest
{
    [Theory(DisplayName = "Store w/ given ID"), Integration]
    public async Task StoreId(IDocumentStorage subject)
    {
        var id = Guid.NewGuid().ToString();
        const string name = "My own ID test";
        var stored = new Category(id, name);

        await subject.Store(stored);
        stored.Id.Should().Be(id);

        var retrieved = await subject.Retrieve<Category>(id);
        retrieved.Should().BeEquivalentTo(stored);
    }

    [Theory(DisplayName = "Retrieve Missing returns null"), Integration]
    public async Task Missing(IDocumentStorage subject)
    {
        var id = Guid.NewGuid().ToString();

        var retrieved = await subject.Retrieve<Category>(id);

        retrieved.Should().BeNull();
    }

    class HasDynamic : IDocumentEntity
    {
        public string? Id { get; set; }
        public dynamic Metadata { get; set; } = new ExpandoObject();
    }

    [Theory(DisplayName = "Store Dynamic"), Integration]
    public async Task StoreDynamic(IDocumentStorage subject)
    {
        var entity = new HasDynamic
        {
            Metadata = new
            {
                Location = "Houston"
            }
        };
        // entity.Metadata.Value = 42;
        ((string)entity.Metadata.Location).Should().Be("Houston");

        await subject.Store(entity);
        entity.Id.Should().NotBeNull();
        var retrieved = await subject.Retrieve<HasDynamic>(entity.Id!);

        retrieved!.Should().NotBeNull();
        ((object)retrieved!.Metadata)!.Should().NotBeNull();
        ((string?)retrieved.Metadata.Location).Should().Be("Houston");

        retrieved.Metadata.Value = 42;
        await subject.Store(retrieved);
        var retrieved2 = await subject.Retrieve<HasDynamic>(entity.Id!);

        retrieved2!.Should().NotBeNull();
        ((object)retrieved2!.Metadata)!.Should().NotBeNull();
        ((string?)retrieved2.Metadata?.Location).Should().Be("Houston");
        ((int?)retrieved2.Metadata?.Value).Should().Be(42);
    }
}
