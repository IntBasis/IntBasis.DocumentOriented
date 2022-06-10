
using Raven.Client.Documents;

namespace IntBasis.DocumentOriented.RavenDB;

public static class RavenDbExample
{
    public static void GettingStarted()
    {
        // https://ravendb.net/docs/article-page/5.3/csharp/start/getting-started#documentstore
        using IDocumentStore store = new DocumentStore
        {
            Urls = new[]                        // URL to the Server,
            {                                   // or list of URLs 
                "http://live-test.ravendb.net"  // to all Cluster Servers (Nodes)
            },
            Database = "Northwind",             // Default database that DocumentStore will interact with
            Conventions = { }                   // DocumentStore customizations
        };
        store.Initialize();                 // Each DocumentStore needs to be initialized before use.
                                            // This process establishes the connection with the Server
                                            // and downloads various configurations
                                            // e.g. cluster topology or client configuration
    }

}
