namespace IntBasis.DocumentOriented;

/// <summary>
/// Provides a way to subscribe to notifications of changes to Document Entities
/// </summary>
public interface IDocumentChanges
{
    /// <summary>
    /// Subscribes to changes to Document Entities of type <typeparamref name="T"/>
    /// and invokes <paramref name="observer"/> for each change.
    /// <para/>
    /// The subscription is closed when the returned object is disposed.
    /// </summary>
    /// <typeparam name="T">The Document Entity type</typeparam>
    /// <param name="observer">The delegate that is called for each change</param>
    /// <returns>A subscription which can be closed by calling <see cref="IDisposable.Dispose"/></returns>
    IDisposable Subscribe<T>(Func<DocumentChangeInfo, Task> observer) where T : class, IDocumentEntity;
}
