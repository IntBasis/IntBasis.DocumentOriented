using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

public record Category(string? Id = null) : IDocumentEntity
{
    public string? Name { get; set; }
}

class Product : IDocumentEntity
{
    public string? Id { get; init; }
    public string? Name { get; internal set; }
    public string? Category { get; internal set; }
    public int UnitsInStock { get; internal set; }
}

// TODO: Use Raven Test Driver https://ravendb.net/docs/article-page/5.3/csharp/start/test-driver

public class RavenDbExampleTest
{
    //public RavenDbExampleTest()
    //{
    //    var store = RavenDbExample.InitializeDocumentStore();
    //    const string DatabaseName = "Test";
    //    var deleteOperation = new DeleteDatabasesOperation(DatabaseName, hardDelete: true, fromNode: null, timeToWaitForConfirmation: null);
    //    store.Maintenance.Server.Send<DeleteDatabaseResult>(deleteOperation);
    //    var createOperation = new CreateDatabaseOperation(new DatabaseRecord(DatabaseName));
    //    store.Maintenance.Server.Send<DatabasePutResult>(createOperation);
    //}

    RavenDbConfiguration TestConfig => new("Test", "http://127.0.0.1:8080");
    IDocumentStore DocumentStore() => RavenDbInitialization.InitializeDocumentStore(TestConfig);

    [Fact(DisplayName = "RavenDB Store")]
    public void Storage()
    {
        var store = DocumentStore();
        using var session = store.OpenSession();
        var category = new Category
        {
            Name = "Test Database Category"
        };
        session.Store(category);
        // Assign an 'Id' and collection (Categories)
        // and start tracking an entity
        category.Id.Should().NotBeNullOrEmpty();

        var product = new Product
        {
            Name = "Test Product",
            Category = category.Id,
            UnitsInStock = 321
        };
        // Assign an 'Id' and collection (Products)
        // and start tracking an entity
        session.Store(product);
        product.Id.Should().NotBeNullOrEmpty();

        session.SaveChanges();
    }

    [Fact(DisplayName = "RavenDB Async Session")]
    public async Task AsyncStorage()
    {
        var store = DocumentStore();
        using var session1 = store.OpenAsyncSession();
        var category = new Category
        {
            Name = "Test Async Category"
        };
        await session1.StoreAsync(category);
        category.Id.Should().NotBeNullOrEmpty();
        await session1.SaveChangesAsync();

        using var session2 = store.OpenAsyncSession();
        var loaded = await session2.LoadAsync<Category>(category.Id);
        loaded.Name.Should().Be(category.Name);
    }

    [Fact(DisplayName = "RavenDB Custom ID")]
    public void CustomId()
    {
        var store = DocumentStore();
        using var session1 = store.OpenSession();
        var category = new Category("my-custom-id") { Name = "expected" };
        session1.Store(category);
        session1.SaveChanges();

        var session2 = store.OpenSession();
        var retrieved = session2.Load<Category>("my-custom-id");
        retrieved.Name.Should().Be("expected");
    }

    [Fact(DisplayName = "RavenDB Read/Modify")]
    public void ReadModify()
    {
        var store = DocumentStore();
        using var session = store.OpenSession();
        var productId = "products/1-A";
        var product = session.Include<Product>(x => x.Category)
                             .Load(productId);
        product.Id.Should().Be(productId);

        var category = session.Load<Category>(product.Category);
        category.Id.Should().Be(product.Category);

        // Make changes
        product.Name = "Changed Product";
        category.Name = "Changed Category";

        session.SaveChanges();
    }
}
