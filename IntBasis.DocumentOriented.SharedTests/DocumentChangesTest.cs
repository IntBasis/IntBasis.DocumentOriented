#if MONGO_DB
namespace IntBasis.DocumentOriented.MongoDB.Tests;
#else
namespace IntBasis.DocumentOriented.RavenDB.Tests;
#endif

public class DocumentChangesTest
{
    // Separate entity type for change tests so we don't pick up changes from other collections
    class TestBook : IDocumentEntity
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public int PageCount { get; set; }
    }

    [Theory(DisplayName = "Subscribe nothing changed"), Integration]
    public async Task NoChanges(IDocumentChanges subject)
    {
        var invoked = false;
        var subscription = subject.Subscribe<TestBook>(_ =>
        {
            invoked = true;
            return Task.CompletedTask;
        });
        await Task.Delay(100);
        subscription.Dispose();
        invoked.Should().BeFalse();
    }

    [Theory(DisplayName = "Subscribe 1 Change"), Integration]
    public async Task OneChange(IDocumentChanges subject, IDocumentStorage documentStorage)
    {
        var entity = new TestBook { Title = "Original Title" };
        await documentStorage.Store(entity);
        DocumentChangeInfo? documentChangeInfo = null;
        using var subscription = subject.Subscribe<TestBook>(d =>
        {
            documentChangeInfo = d;
            return Task.CompletedTask;
        });

        entity.Title = "New title";
        await documentStorage.Store(entity);

        await Task.Delay(100);
        documentChangeInfo?.DocumentId.Should().Be(entity.Id);
    }

    [Theory(DisplayName = "Subscribe 1 New Entity"), Integration]
    public async Task NewEntity(IDocumentChanges subject, IDocumentStorage documentStorage)
    {
        DocumentChangeInfo? documentChangeInfo = null;
        using var subscription = subject.Subscribe<TestBook>(d =>
        {
            documentChangeInfo = d;
            return Task.CompletedTask;
        });
        var entity = new TestBook { Title = "New Book" };

        await documentStorage.Store(entity);

        await Task.Delay(100);
        documentChangeInfo?.DocumentId.Should().Be(entity.Id);
    }

    [Theory(DisplayName = "Subscribe 5 Changes"), Integration]
    public async Task FiveChanges(IDocumentChanges subject, IDocumentStorage documentStorage)
    {
        var entity = new TestBook();
        await documentStorage.Store(entity);
        var invoked = 0;
        using var subscription = subject.Subscribe<TestBook>(_ =>
        {
            invoked++;
            return Task.CompletedTask;
        });

        for (int i = 1; i <= 5; i++)
        {
            entity.PageCount = i;
            await documentStorage.Store(entity);
        }

        await Task.Delay(600);
        invoked.Should().Be(5);
    }

    [Theory(DisplayName = "Disposal ends subscription"), Integration]
    public async Task Disposal(IDocumentChanges subject, IDocumentStorage documentStorage)
    {
        var invoked = false;
        var subscription = subject.Subscribe<TestBook>(_ =>
        {
            invoked = true;
            return Task.CompletedTask;
        });
        subscription.Dispose();

        await documentStorage.Store(new TestBook());

        await Task.Delay(200);
        invoked.Should().BeFalse();
    }
}
