using IntBasis.DocumentOriented;
using IntBasis.DocumentOriented.LiteDB;

// .NET Practice is to place ServiceCollectionExtensions in the following namespace
// to improve discoverability of the extension method during service configuration
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds LiteDB implementation of Document Oriented Database services.
    /// <para/>
    /// External:
    /// <list type="bullet">
    /// <item> <see cref="IDocumentChanges"/> </item>
    /// <item> <see cref="IDocumentStorage"/> </item>
    /// <item> <see cref="IDocumentQuery"/> </item>
    /// </list>
    /// </summary>
    public static IServiceCollection AddDocumentOrientedLiteDb(this IServiceCollection services,
                                                               LiteDbConfiguration configuration)
    {
        services.AddSingleton(configuration);
        //services.AddTransient<IDocumentChanges, LiteDbDocumentChanges>();
        //services.AddTransient<IDocumentStorage, LiteDbDocumentStorage>();
        //services.AddTransient<IDocumentQuery, LiteDbDocumentQuery>();
        return services;
    }
}
