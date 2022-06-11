using FluentAssertions;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

class Category
{
    public string Id { get; set; }
    public string Name { get; set; }
}

class Product
{
    public string Id { get; set; }
    public string Name { get; internal set; }
    public string Category { get; internal set; }
    public int UnitsInStock { get; internal set; }
}

public class RavenDbExampleTest
{
    [Fact(DisplayName = "RavenDB Store")]
    public void Storage()
    {
        var store = RavenDbExample.InitializeDocumentStore();
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

    [Fact(DisplayName = "RavenDB Read/Modify")]
    public void ReadModify()
    {
        var store = RavenDbExample.InitializeDocumentStore();
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
