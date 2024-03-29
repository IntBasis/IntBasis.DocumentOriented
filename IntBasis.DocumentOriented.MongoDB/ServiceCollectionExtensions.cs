﻿using IntBasis.DocumentOriented;
using IntBasis.DocumentOriented.MongoDB;

// .NET Practice is to place ServiceCollectionExtensions in the following namespace
// to improve discoverability of the extension method during service configuration
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds MongoDB implementation of Document Oriented Database services.
    /// <para/>
    /// External:
    /// <list type="bullet">
    /// <item> <see cref="IDocumentChanges"/> </item>
    /// <item> <see cref="IDocumentStorage"/> </item>
    /// <item> <see cref="IDocumentQuery"/> </item>
    /// </list>
    /// <para/>
    /// Internal:
    /// <list type="bullet">
    /// <item> <see cref="IMongoDatabaseService"/> </item>
    /// <item> <see cref="IMongoCollectionService"/> </item>
    /// </list>
    /// </summary>
    public static IServiceCollection AddDocumentOrientedMongoDb(this IServiceCollection services,
                                                                MongoDbConfiguration configuration)
    {
        services.AddSingleton(configuration);
        services.AddTransient<IDocumentChanges, MongoDbDocumentChanges>();
        services.AddTransient<IDocumentStorage, MongoDbDocumentStorage>();
        services.AddTransient<IDocumentQuery, MongoDbDocumentQuery>();
        services.AddTransient<IMongoDatabaseService, MongoDatabaseService>();
        services.AddTransient<IMongoCollectionService, MongoCollectionService>();
        return services;
    }
}
