namespace IntBasis.DocumentOriented.Testing;

public static class TypeExtensions
{
    /// <summary>
    /// Returns a default value for a given Type at run-time.
    /// </summary>
    public static object? GetDefaultValue(this Type type)
    {
        // https://stackoverflow.com/a/2686723/483776
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }
        else
        {
            return null;
        }
    }

    public static bool CanDefaultConstruct(this Type type)
    {
        return !type.IsAbstract && type.HasParameterlessConstructor();
    }

    /// <summary>
    /// Gets a value indicating if the given type has a parameterless constructor.
    /// True if it has a parameterless constructor, otherwise false.
    /// </summary>
    /// <param name="type">The type.</param>
    public static bool HasParameterlessConstructor(this Type type)
    {
        // https://github.com/JoshClose/CsvHelper/blob/4cf576ec5524f73fc7894232b23a5969acc48e46/src/CsvHelper/ReflectionExtensions.cs
        return type.GetConstructor(Array.Empty<Type>()) is not null;
    }
}
