﻿namespace IntBasis.DocumentOriented;

/// <summary>
/// Encapsulates the simple storage and retrieval of individual document entities by Id.
/// </summary>
public interface IDocumentStorage
{
    /// <summary>
    /// Store (upsert) the given entity in configured Document storage.
    /// The <see cref="IDocumentEntity.Id"/> is set to a unique identifier (if it was not already set).
    /// <para/>
    /// If the provided <see cref="IDocumentEntity.Id"/> is already in storage
    /// then the entity will be replaced, effectively performing an update.
    /// </summary>
    Task Store<T>(T entity) where T : IDocumentEntity;

    /// <summary>
    /// Retrieve the document entity from storage by the given <paramref name="id"/>.
    /// <para/>
    /// Returns null if the entity is not found.
    /// </summary>
    Task<T?> Retrieve<T>(string id) where T : IDocumentEntity;
}
