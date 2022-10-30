using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace IntBasis.DocumentOriented.RavenDB;

internal static class RavenDbInitialization
{
    internal static IDocumentStore InitializeDocumentStore(RavenDbConfiguration configuration)
    {
        // The DocumentStore is capable of working with multiple databases and for proper operation we recommend having only one instance of it per application.
        // https://ravendb.net/docs/article-page/5.3/csharp/start/getting-started#documentstore
        IDocumentStore documentStore = new DocumentStore
        {
            Urls = configuration.ServerUrls,
            Database = configuration.DatabaseName
        };
        if (configuration.MaxNumberOfRequestsPerSession.HasValue)
        {
            // Allow overriding max requests per session (primarily for testing)
            documentStore.Conventions.MaxNumberOfRequestsPerSession = configuration.MaxNumberOfRequestsPerSession.Value;
        }
        documentStore.Conventions.Serialization = new NewtonsoftJsonSerializationConventions
        {
            // HACK: Prevent serializing `$type` member e.g. on ExpandObjects
            //       so that stored entity and retrieved entity are equivalent
            CustomizeJsonSerializer = serializer => serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None
        };
        // Each DocumentStore needs to be initialized before use.
        // This process establishes the connection with the Server
        // and downloads various configurations
        // e.g. cluster topology or client configuration
        documentStore.Initialize();
        CreateDatabaseIfDoesNotExist(configuration, documentStore);
        return documentStore;
    }

    /// <summary>
    /// Opens a synchronous Session using the configured <see cref="IDocumentStore"/>.
    /// The Session should be disposed by the caller.
    /// The lifetime should be for the "request" (i.e. Scoped).
    /// </summary>
    internal static IDocumentSession OpenSession(IServiceProvider sp)
    {
        var documentStore = sp.GetRequiredService<IDocumentStore>();
        return documentStore.OpenSession();
    }

    /// <summary>
    /// Opens an asynchronous Session using the configured <see cref="IDocumentStore"/>.
    /// The Session should be disposed by the caller.
    /// The lifetime should be for the "request" (i.e. Scoped).
    /// </summary>
    internal static IAsyncDocumentSession OpenAsyncSession(IServiceProvider sp)
    {
        var documentStore = sp.GetRequiredService<IDocumentStore>();
        return documentStore.OpenAsyncSession();
    }

    private static void CreateDatabaseIfDoesNotExist(RavenDbConfiguration configuration, IDocumentStore store)
    {
        var exists = DatabaseExists(configuration, store);
        if (!exists)
        {
            var createOperation = new CreateDatabaseOperation(new DatabaseRecord(configuration.DatabaseName));
            store.Maintenance.Server.Send<DatabasePutResult>(createOperation);
        }
    }

    private static bool DatabaseExists(RavenDbConfiguration configuration, IDocumentStore store)
    {
        var getDbOperation = new GetDatabaseRecordOperation(configuration.DatabaseName);
        var databaseRecord = store.Maintenance.Server.Send<DatabaseRecordWithEtag>(getDbOperation);
        return databaseRecord is not null;
    }
}
