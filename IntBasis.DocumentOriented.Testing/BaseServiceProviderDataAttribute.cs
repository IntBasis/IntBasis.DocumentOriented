using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace IntBasis.DocumentOriented.Testing;

/// <summary>
/// Provides way to leverage standard <see cref="IServiceCollection"/> Configuration
/// to inject services to unit tests.
/// </summary>
public abstract class BaseServiceProviderDataAttribute : DataAttribute
{
    /// <summary>
    /// If true, will throw an exception when a service cannot be resolved.
    /// If false it will just return null for unknown services.
    /// </summary>
    private readonly bool shouldThrow = true;

    protected BaseServiceProviderDataAttribute()
    {
    }

    protected BaseServiceProviderDataAttribute(bool shouldThrow)
    {
        this.shouldThrow = shouldThrow;
    }


    protected abstract void ConfigureServices(IServiceCollection services);

    public override IEnumerable<object?[]> GetData(MethodInfo testMethod)
    {
        var parameters = GetServices(testMethod);
        return new[] { parameters };
    }

    private object?[] GetServices(MethodBase method)
    {
        var serviceProvider = GetServiceProviderInstance();
        return method.GetParameters()
                     .Select(p => GetService(p, serviceProvider))
                     .ToArray();
    }

    private object? GetService(ParameterInfo p, IServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetService(p.ParameterType) ?? GetTestService(p);
        if (service is null && shouldThrow)
            throw new Exception($"'{p.ParameterType.Name} {p.Name}': Unable to resolve a test service for the parameter.");
        return service;
    }

    private object? GetTestService(ParameterInfo p)
    {
        var type = p.ParameterType;
        // Return null for primitives and strings because they shouldn't be injected via service provider
        // (It's likely checking for string / primitives is being done in conjunection with a InlineData and CompopsiteData attributes)
        if (type.IsPrimitive || type == typeof(string))
            return null;
        // HACK: `null` is a valid value for IProgress<T>, but providing a mock to keep it simple to know when a service could not be resolved
        if (type == typeof(IProgress<double>))
            return Mock.Of<IProgress<double>>();
        // If it has a default constructor just construct and return
        if (type.CanDefaultConstruct())
            return Activator.CreateInstance(type);
        // Otherwise let's try to recurse getting required parameters for a constructor
        if (!type.IsAbstract && type.GetConstructors().Any())
        {
            // HACK: Just try the first constructor...
            var constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).First();
            var constructorArguments = GetServices(constructor);  // Recursion
            return constructor.Invoke(constructorArguments);
        }
        return null;
    }

    private IServiceProvider GetServiceProviderInstance()
    {
        return serviceProviderInstance ??= BuildServiceProvider();
    }

    private IServiceProvider? serviceProviderInstance;

    private IServiceProvider BuildServiceProvider()
    {
        //var configuration = LoadConfiguration();
        //var startup = new Startup(configuration);
        var services = new ServiceCollection();
        ConfigureServices(services);
        return services.BuildServiceProvider();
    }
}
