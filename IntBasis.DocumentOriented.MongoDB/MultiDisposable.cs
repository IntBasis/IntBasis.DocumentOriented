namespace IntBasis.DocumentOriented.MongoDB;

class MultiDisposable : IDisposable
{
    private readonly IDisposable[] disposables;

    public MultiDisposable(params IDisposable[] disposables)
    {
        this.disposables = disposables ?? throw new ArgumentNullException(nameof(disposables));
    }

    public void Dispose()
    {
        foreach (var disposable in disposables)
            disposable.Dispose();
    }
}
