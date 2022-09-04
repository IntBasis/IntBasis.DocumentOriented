using Raven.Client.Documents;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

public class RavenDbDocumentStorageTest
{
    [Theory(DisplayName = "Store"), Integration]
    public async Task Store(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        var expected = Guid.NewGuid().ToString();
        var entity = new Category(expected);

        await subject.Store(entity);
        entity.Id.Should().NotBeNullOrEmpty();

        // Verify against separate manually created Session
        using var session = underlyingStore.OpenSession();
        var retrieved = session.Load<Category>(entity.Id);
        retrieved.Should().NotBeNull();
        retrieved.Name.Should().Be(expected);
    }

    [Theory(DisplayName = "Retrieve"), Integration]
    public async Task Retrieve(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        var expected = Guid.NewGuid().ToString();
        var stored = new Category(expected);
        // Store via separate manually created Session
        using var session = underlyingStore.OpenSession();
        session.Store(stored);
        session.SaveChanges();

        var retrieved = await subject.Retrieve<Category>(stored.Id);

        retrieved.Should().BeEquivalentTo(stored);
    }

    [Theory(DisplayName = "Store Twice"), Integration]
    public async Task StoreTwice(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        // Storing something twice should replace the first one
        var expected = Guid.NewGuid().ToString();
        var entity = new Category("Original name (not expected)");
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

    [Theory(DisplayName = "IgnoreChanges: Initial Store"), Integration]
    public async Task IgnoreChanges(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        // RavenDB Sessions automatically track changes for all returned entities
        // We do not want to save changes to Entity A when Entity B is being stored
        // This side effect would be a surprise to the consumer of RavenDbDocumentStorage
        var expected = "Initial Name (expected)";
        var notExpected = "Changed Name (should not be saved)";
        var entity = new Category(expected);
        await subject.Store(entity);

        entity.Name = notExpected;
        await subject.Store(new Category("Another unrelated entity"));

        // Verify against separate manually created Session
        using var session = underlyingStore.OpenSession();
        var retrieved = session.Load<Category>(entity.Id);
        retrieved.Should().NotBeNull();
        retrieved.Name.Should().Be(expected);
    }

    [Theory(DisplayName = "IgnoreChanges: Retrieved"), Integration]
    public async Task IgnoreChangesRetrieve(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        // RavenDB Sessions automatically track changes for all returned entities
        // We do not want to save changes to Entity A when Entity B is being stored
        // This side effect would be a surprise to the consumer of RavenDbDocumentStorage
        var expected = "Initial Name (expected)";
        var notExpected = "Changed Name (should not be saved)";
        var entity = new Category(expected);
        // Store via separate manually created Session
        using (var session = underlyingStore.OpenSession())
        {
            session.Store(entity);
            session.SaveChanges();
        }

        // Act: Retrieve and change retrieved record, then store a different entity
        var retrieved = await subject.Retrieve<Category>(entity.Id);
        retrieved.Name = notExpected;
        await subject.Store(new Category("Another unrelated entity"));

        // Verify against separate manually created Session
        using var session2 = underlyingStore.OpenSession();
        var stored = session2.Load<Category>(entity.Id);
        stored.Name.Should().Be(expected);
    }
}
