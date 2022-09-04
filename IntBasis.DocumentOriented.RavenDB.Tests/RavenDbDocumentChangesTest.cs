namespace IntBasis.DocumentOriented.RavenDB.Tests;

public class RavenDbDocumentChangesTest
{
    // Separate entity type for change tests so we don't pick up changes from other collections
    class TestBook : IDocumentEntity
    {
        public string? Id {get;set;}
        public string? Title { get; set; }
        public int PageCount { get; set; }
    }

    [Theory(DisplayName = "Subscribe nothing changed"), Integration]
    public async Task NoChanges(IDocumentChanges subject)
    {
        var invoked = false;
        var subscription = subject.Subscribe<TestBook>(() =>
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
        var entity = new TestBook();
        await documentStorage.Store(entity);
        var invoked = false;
        using var subscription = subject.Subscribe<TestBook>(() =>
        {
            invoked = true;
            return Task.CompletedTask;
        });

        entity.Title = "New title";
        await documentStorage.Store(entity);

        await Task.Delay(100);
        invoked.Should().BeTrue();
    }

    [Theory(DisplayName = "Subscribe 5 Changes"), Integration]
    public async Task FiveChanges(IDocumentChanges subject, IDocumentStorage documentStorage)
    {
        var entity = new TestBook();
        await documentStorage.Store(entity);
        var invoked = 0;
        using var subscription = subject.Subscribe<TestBook>(() =>
        {
            invoked++;
            return Task.CompletedTask;
        });

        for (int i = 0; i < 5; i++)
        {
            entity.PageCount = i;
            await documentStorage.Store(entity);
        }

        await Task.Delay(500);
        invoked.Should().BeInRange(5, 6); // TODO: Figure out why sometimes 6
    }

    [Theory(DisplayName = "Disposal ends subscription"), Integration]
    public async Task Disposal(IDocumentChanges subject, IDocumentStorage documentStorage)
    {
        var invoked = false;
        var subscription = subject.Subscribe<TestBook>(() =>
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
