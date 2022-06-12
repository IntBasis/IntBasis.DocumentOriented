﻿using Microsoft.Extensions.DependencyInjection;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

public class Integration : BaseServiceProviderDataAttribute
{
    RavenDbConfiguration TestConfig => new("Test", "http://127.0.0.1:8080");

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDocumentOrientedRavenDb(TestConfig);
    }
}