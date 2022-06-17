using IntBasis.DocumentOriented.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntBasis.DocumentOriented.Tests;

public class DictionaryStorageAttribute : BaseServiceProviderDataAttribute
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        // HACK: Using singletons so it is always "frozen" and getting same service each time (to enable easy setup)
        services.AddSingleton<ISchemaVersionService, DictionarySchemaVersionService>()
                .AddSingleton<IDocumentStorage, DictionaryDocumentStorage>()
                .AddSingleton<ISchemaVersionedDocumentStorage, SchemaVersionedDocumentStorage>()
                // For accessing the dictionary implementations for easy setup
                .AddTransient(sp => (DictionarySchemaVersionService)sp.GetRequiredService<ISchemaVersionService>())
                .AddTransient(sp => (DictionaryDocumentStorage)sp.GetRequiredService<IDocumentStorage>());
    }
}
