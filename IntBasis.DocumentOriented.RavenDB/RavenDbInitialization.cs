using Raven.Client.Documents;

namespace IntBasis.DocumentOriented.RavenDB;

internal static class RavenDbInitialization
{
    public static IDocumentStore InitializeDocumentStore(RavenDbConfiguration configuration)
    {
        // The DocumentStore is capable of working with multiple databases and for proper operation we recommend having only one instance of it per application.
        // https://ravendb.net/docs/article-page/5.3/csharp/start/getting-started#documentstore
        IDocumentStore store = new DocumentStore
        {
            Urls = configuration.ServerUrls,
            Database = configuration.DatabaseName
        };
        // Each DocumentStore needs to be initialized before use.
        // This process establishes the connection with the Server
        // and downloads various configurations
        // e.g. cluster topology or client configuration
        store.Initialize();                 
        return store;
    }
}
