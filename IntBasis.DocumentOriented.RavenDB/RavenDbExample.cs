using Raven.Client.Documents;

namespace IntBasis.DocumentOriented.RavenDB;

public static class RavenDbExample
{
    public static IDocumentStore InitializeDocumentStore()
    {
        // The DocumentStore is capable of working with multiple databases and for proper operation we recommend having only one instance of it per application.
        // https://ravendb.net/docs/article-page/5.3/csharp/start/getting-started#documentstore
        IDocumentStore store = new DocumentStore
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
        return store;
    }
}
