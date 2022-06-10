namespace IntBasis.DocumentOriented;

/// <summary>
/// Has a <see cref="SchemaVersion"/> property for tracking the version of the schema at the time the resource was persisted.
/// </summary>
public interface IHasSchemaVersion
{
    /// <summary>
    /// The Schema Version for this Document
    /// </summary>
    public int SchemaVersion { get; set; }
}
