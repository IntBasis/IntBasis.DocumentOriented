using IntBasis.DocumentOriented.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntBasis.DocumentOriented.LiteDB.Tests;

public class IntegrationAttribute : BaseServiceProviderDataAttribute
{
    LiteDbConfiguration TestConfig => new(fileName: ":temp:");

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDocumentOrientedLiteDb(TestConfig);
    }
}
