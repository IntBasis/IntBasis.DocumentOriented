namespace IntBasis.DocumentOriented;

/// <summary>
/// Responsible for supplying the Schema Version for given types.
/// </summary>
public interface ISchemaVersionService
{
    public int GetCurrentSchemaVersion<T>() where T : ISchemaVersionedDocumentEntity;
    //public int GetCurrentSchemaVersion(Type type);
}
