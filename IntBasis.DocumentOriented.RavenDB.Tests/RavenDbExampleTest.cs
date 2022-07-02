using System.Dynamic;
using IntBasis.DocumentOriented.Testing;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

//public record Category(string? Id = null) : IDocumentEntity
//{
//    public string? Name { get; set; }
//}

class Category : IDocumentEntity
{
    public string? Id { get; set; }
    public string? Name { get; internal set; }

    public Category()
    {
    }

    public Category(string? id, string? name)
    {
        Id = id;
        Name = name;
    }
}

class Product : IDocumentEntity
{
    public string? Id { get; set; }
    public string? Name { get; internal set; }
    public string? Category { get; internal set; }
    public int UnitsInStock { get; internal set; }
}
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

    [Theory(DisplayName = "RavenDB Store"), Integration]
    public void Storage(IDocumentSession session)
    {
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

    [Theory(DisplayName = "RavenDB Async Session"), Integration]
    public async Task AsyncStorage(IAsyncDocumentSession session1, IAsyncDocumentSession session2)
    {
        var category = new Category
        {
            Name = "Test Async Category"
        };
        await session1.StoreAsync(category);
        category.Id.Should().NotBeNullOrEmpty();
        await session1.SaveChangesAsync();

        var loaded = await session2.LoadAsync<Category>(category.Id);
        loaded.Name.Should().Be(category.Name);
    }

    [Theory(DisplayName = "RavenDB Custom ID"), AutoMoq]
    public void CustomId(string customId, string expected)
    {
        var store = DocumentStore();
        using var session1 = store.OpenSession();
        var category = new Category(customId, expected);
        session1.Store(category);
        session1.SaveChanges();

        var session2 = store.OpenSession();
        var retrieved = session2.Load<Category>(customId);
        retrieved.Name.Should().Be(expected);
    }

    [Theory(DisplayName = "RavenDB Read/Modify"), Integration]
    public void ReadModify(IDocumentSession session)
    {
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

    [Theory(DisplayName = "Store Dynamic"), Integration]
    public async Task StoreDynamic(RavenDbDocumentStorage subject)
    {
        await CommonDocumentStorageTest.VerifyDynamicObjectStorage(subject);
    }
}
