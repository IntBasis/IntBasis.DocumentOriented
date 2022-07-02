using IntBasis.DocumentOriented.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntBasis.DocumentOriented.MongoDB.Tests;

public class IntegrationAttribute : BaseServiceProviderDataAttribute
{
    MongoDbConfiguration TestConfig => new(connectionString: "mongodb://localhost:27017",
                                           databaseName: "test");

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDocumentOrientedMongoDb(TestConfig);
    }
}
