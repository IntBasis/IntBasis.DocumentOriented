using Raven.Client.Documents;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

public class RavenDbDocumentStorageTest
{
    [Theory(DisplayName = "Store"), Integration]
    public async Task Store(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
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

   [Theory(DisplayName = "Store w/ given ID"), Integration]
    public async Task StoreId(RavenDbDocumentStorage subject)
    {
        var id = Guid.NewGuid().ToString();
        const string name = "My own ID test";
        var stored = new Category(id, name);
        await subject.Store(stored);
        stored.Id.Should().Be(id);

        var retrieved = await subject.Retrieve<Category>(id);
        retrieved.Should().BeEquivalentTo(stored);
    }

    [Theory(DisplayName = "Retrieve"), Integration]
    public async Task Retrieve(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
    {
        var expected = Guid.NewGuid().ToString();
        var stored = new Category { Name = expected };
        // Store via separate manually created Session
        using var session = underlyingStore.OpenSession();
        session.Store(stored);
        session.SaveChanges();

        var retrieved = await subject.Retrieve<Category>(stored.Id);

        retrieved.Should().BeEquivalentTo(stored);
    }

    [Theory(DisplayName = "Retrieve Missing returns null"), Integration]
    public async Task Missing(RavenDbDocumentStorage subject)
    {
        var id = Guid.NewGuid().ToString();

        var retrieved = await subject.Retrieve<Category>(id);

        retrieved.Should().BeNull();
    }

    [Theory(DisplayName = "Store Twice"), Integration]
    public async Task StoreTwice(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
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

    [Theory(DisplayName = "IgnoreChanges: Initial Store"), Integration]
    public async Task IgnoreChanges(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
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
    public async Task IgnoreChangesRetrieve(RavenDbDocumentStorage subject, IDocumentStore underlyingStore)
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
}
