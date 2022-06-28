using MongoDB.Driver;

namespace IntBasis.DocumentOriented.MongoDB.Tests;

class Category : IDocumentEntity
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

public class MongoDbDocumentStorageTest
{
    [Theory(DisplayName = "Store"), Integration]
    public async Task Storage(MongoDbDocumentStorage subject, IMongoDatabaseService mongoDatabaseService)
    {
        const string name = "storage test";
        var entity = new Category(name);
        await subject.Store(entity);
        entity.Id.Should().NotBeNull();

        var mongoDatabase = mongoDatabaseService.GetDatabase();
        var collectionName = "categories";
        var collection = mongoDatabase.GetCollection<Category>(collectionName);
        var found = collection.Find<Category>(doc => doc.Id == entity.Id)
                              .FirstOrDefault();
        found.Id.Should().Be(entity.Id);
        found.Name.Should().Be(name);
    }

    [Theory(DisplayName = "Store w/ given ID"), Integration]
    public async Task StoreId(MongoDbDocumentStorage subject)
    {
        var id = Guid.NewGuid().ToString();
        const string name = "My own ID test";
        var stored = new Category(id, name);
        await subject.Store(stored);
        stored.Id.Should().Be(id);

        var retrieved = await subject.Retrieve<Category>(id);
        retrieved.Should().BeEquivalentTo(stored);
    }

    [Theory(DisplayName = "Retrieve"), Integration]
    public async Task Retrieval(MongoDbDocumentStorage subject, IMongoDatabaseService mongoDatabaseService)
    {
        var mongoDatabase = mongoDatabaseService.GetDatabase();
        var collectionName = "categories";
        var collection = mongoDatabase.GetCollection<Category>(collectionName);
        var id = Guid.NewGuid().ToString();
        const string name = "retrieval test";
        var inserted = new Category(id, name);
        collection.InsertOne(inserted);

        var retrieved = await subject.Retrieve<Category>(id);

        retrieved.Should().BeEquivalentTo(inserted);
    }

    [Theory(DisplayName = "Retrieve Missing"), Integration]
    public async Task RetrieveNull(MongoDbDocumentStorage subject)
    {
        var id = Guid.NewGuid().ToString();

        var retrieved = await subject.Retrieve<Category>(id);

        retrieved.Should().BeNull();
    }
}
