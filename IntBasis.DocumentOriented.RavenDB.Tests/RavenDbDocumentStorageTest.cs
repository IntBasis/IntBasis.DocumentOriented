using Raven.Client.Documents;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

public class RavenDbDocumentStorageTest
{
    [Theory(DisplayName = "Store"), Integration]
    public async Task StoreAsync(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        var expected = Guid.NewGuid().ToString();
        var entity = new Category { Name = expected };

        await subject.Store(entity);
        entity.Id.Should().NotBeNullOrEmpty();

        // Verify against separate manually created Session
        using var session = underlyingStore.OpenSession();
        var retrieved = session.Load<Category>(entity.Id);
        retrieved.Should().NotBeNull();
        retrieved.Name.Should().Be(expected);
    }

    [Theory(DisplayName = "Retrieve"), Integration]
    public async Task RetrieveAsync(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        var expected = Guid.NewGuid().ToString();
        var entity = new Category { Name = expected };
        // Store via separate manually created Session
        using var session = underlyingStore.OpenSession();
        session.Store(entity);
        session.SaveChanges();

        var retrieved = await subject.Retrieve<Category>(entity.Id);
        retrieved.Name.Should().Be(expected);
    }

    [Theory(DisplayName = "Retrieve Missing returns null"), Integration]
    public async Task Missing(RavenDbDocumentStorage subject)
    {
        var retrieved = await subject.Retrieve<Category>("1");
        retrieved.Should().BeNull();
    }

    [Theory(DisplayName = "IgnoreChanges: Initial Store"), Integration]
    public async Task IgnoreChangesAsync(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        // RavenDB Sessions automatically track changes for all returned entities
        // We do not want to save changes to Entity A when Entity B is being stored
        // This side effect would be a surprise to the consumer of RavenDbDocumentStorage
        var expected = "Initial Name (expected)";
        var notExpected = "Changed Name (should not be saved)";
        var entity = new Category { Name = expected };
        await subject.Store(entity);

        entity.Name = notExpected;
        await subject.Store(new Category { Name = "Another unrelated entity" });

        // Verify against separate manually created Session
        using var session = underlyingStore.OpenSession();
        var retrieved = session.Load<Category>(entity.Id);
        retrieved.Should().NotBeNull();
        retrieved.Name.Should().Be(expected);
    }

    [Theory(DisplayName = "IgnoreChanges: Retrieved"), Integration]
    public async Task IgnoreChangesRetrieveAsync(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        // RavenDB Sessions automatically track changes for all returned entities
        // We do not want to save changes to Entity A when Entity B is being stored
        // This side effect would be a surprise to the consumer of RavenDbDocumentStorage
        var expected = "Initial Name (expected)";
        var notExpected = "Changed Name (should not be saved)";
        var entity = new Category { Name = expected };
        // Store via separate manually created Session
        using (var session = underlyingStore.OpenSession())
        {
            session.Store(entity);
            session.SaveChanges();
        }

        // Act: Retrieve and change retrieved record, then store a different entity
        var retrieved = await subject.Retrieve<Category>(entity.Id);
        retrieved.Name = notExpected;
        await subject.Store(new Category { Name = "Another unrelated entity" });

        // Verify against separate manually created Session
        using var session2 = underlyingStore.OpenSession();
        var stored = session2.Load<Category>(entity.Id);
        stored.Name.Should().Be(expected);
    }

    [Theory(DisplayName = "Store Twice"), Integration]
    public async Task StoreTwiceAsync(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        // Storing something twice should replace the first one
        var expected = Guid.NewGuid().ToString();
        var entity = new Category { Name = "Original name (not expected)" };
        await subject.Store(entity);

        // Set the name to the new expected value and Store same object again
        entity.Name = expected;
        await subject.Store(entity);

        // Verify against separate manually created Session
        using var session = underlyingStore.OpenSession();
        var retrieved = session.Load<Category>(entity.Id);
        retrieved.Should().NotBeNull();
        retrieved.Name.Should().Be(expected);
    }
}
