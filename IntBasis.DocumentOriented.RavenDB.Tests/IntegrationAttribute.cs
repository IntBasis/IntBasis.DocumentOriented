using IntBasis.DocumentOriented.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

public class IntegrationAttribute : BaseServiceProviderDataAttribute
{
    internal const string TestServer = "http://127.0.0.1:8080";
    internal static RavenDbConfiguration TestConfig => new("Test", TestServer);

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDocumentOrientedRavenDb(TestConfig);
    }
}
