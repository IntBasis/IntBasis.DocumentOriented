using IntBasis.DocumentOriented.RavenDB;
using static IntBasis.DocumentOriented.RavenDB.RavenDbInitialization;

// .NET Practice is to place ServiceCollectionExtensions in the following namespace
// to improve discoverability of the extension method during service configuration
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds RavenDB implementation of Document Oriented Database services
    /// </summary>
    public static IServiceCollection AddDocumentOrientedRavenDb(this IServiceCollection services,
                                                                RavenDbConfiguration configuration)
    {
        // A single instance of the Document Store (Singleton Pattern)
        // should be created per cluster per the lifetime of your application.
        // See https://ravendb.net/docs/article-page/5.3/csharp/client-api/what-is-a-document-store
        services.AddSingleton(_ => InitializeDocumentStore(configuration));
        // In web applications, a common (and recommended) pattern is to create a session per request.
        // See https://ravendb.net/docs/article-page/5.3/csharp/start/getting-started#session
        return services.AddScoped(OpenSession)
                       .AddScoped(OpenAsyncSession);
    }
}
