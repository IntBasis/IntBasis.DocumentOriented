namespace IntBasis.DocumentOriented;

/// <summary>
/// A Document that has a <see cref="IHasSchemaVersion.SchemaVersion"/> property for tracking the version of the schema at the time the Document was persisted.
/// </summary>
public interface ISchemaVersionedDocumentEntity : IHasSchemaVersion, IDocumentEntity { }
