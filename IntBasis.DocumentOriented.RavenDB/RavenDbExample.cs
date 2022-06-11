using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace IntBasis.DocumentOriented.RavenDB;

public static class RavenDbExample
{
    class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    class Product
    {
        public string Name { get; internal set; }
        public string Category { get; internal set; }
        public int UnitsInStock { get; internal set; }
    }

    public static void GettingStarted()
    {
        // https://ravendb.net/docs/article-page/5.3/csharp/start/getting-started#documentstore
        using IDocumentStore store = new DocumentStore
        {
            // URL to the Server,
            // or list of URLs 
            // to all Cluster Servers (Nodes)
            Urls = new[]
            {                                   
                // Local test server (default port)                                
                "http://127.0.0.1:8080"
            },
            Database = "Test",              // Default database that DocumentStore will interact with
            Conventions = { }               // DocumentStore customizations
        };
        store.Initialize();                 // Each DocumentStore needs to be initialized before use.
                                            // This process establishes the connection with the Server
                                            // and downloads various configurations
                                            // e.g. cluster topology or client configuration
        SaveDocuments(store);

        var productId = "products/1-A";

        using IDocumentSession session = store.OpenSession();  // Open a session for a default 'Database'
        Product product = session
            .Include<Product>(x => x.Category)              // Include Category
            .Load(productId);                               // Load the Product and start tracking

        Category category = session
            .Load<Category>(product.Category);              // No remote calls,
                                                            // Session contains this entity from .Include

        product.Name = "RavenDB";                           // Apply changes
        category.Name = "Database";

        session.SaveChanges();                              // Synchronize with the Server
                                                            // one request processed in one transaction
    }

    private static void SaveDocuments(IDocumentStore store)
    {
        using IDocumentSession session = store.OpenSession();  // Open a session for a default 'Database'
        // For example, in web applications, a common (and recommended) pattern is to create a session per request.
        var category = new Category
        {
            Name = "Database Category"
        };
        session.Store(category);                            // Assign an 'Id' and collection (Categories)
                                                            // and start tracking an entity
        var product = new Product
        {
            Name = "RavenDB Database",
            Category = category.Id,
            UnitsInStock = 10
        };
        session.Store(product);                             // Assign an 'Id' and collection (Products)
                                                            // and start tracking an entity
        session.SaveChanges();                              // Send to the Server
                                                            // one request processed in one transaction
    }
}
