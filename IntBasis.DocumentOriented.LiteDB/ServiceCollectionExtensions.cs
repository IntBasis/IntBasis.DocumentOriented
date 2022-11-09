using IntBasis.DocumentOriented;
using IntBasis.DocumentOriented.LiteDB;
using LiteDB;
using LiteDB.Realtime;

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
    /// Internal:
    /// <list type="bullet">
    /// <item> <see cref="ILiteDatabase"/> </item>
    /// </list>
    /// </summary>
    public static IServiceCollection AddDocumentOrientedLiteDb(this IServiceCollection services,
                                                               LiteDbConfiguration configuration)
    {
        services.AddSingleton(configuration);
        //services.AddScoped<ILiteDatabase>(sp => new LiteDatabase(configuration.FileName));
        // RealtimeLiteDatabase is community contribution for change subscription
        // https://github.com/FuturistiCoder/LiteDB.Realtime
        services.AddScoped(sp => new RealtimeLiteDatabase(configuration.FileName));
        services.AddScoped<ILiteDatabase>(sp => sp.GetRequiredService<RealtimeLiteDatabase>());
        services.AddTransient<IDocumentStorage, LiteDbDocumentStorage>();
        services.AddTransient<IDocumentQuery, LiteDbDocumentQuery>();

        services.AddTransient<IDocumentChanges, LiteDbDocumentChanges>();
        return services;
    }
}
