namespace IntBasis.DocumentOriented.Tests;

public class DictionarySchemaVersionService : ISchemaVersionService
{
    private readonly Dictionary<Type, int> dictionary = new();

    internal void SetSchemaVersion(Type type, int schemaVersion)
    {
        dictionary[type] = schemaVersion;
    }

    public int GetCurrentSchemaVersion(Type type)
    {
        return dictionary[type];
    }
}
