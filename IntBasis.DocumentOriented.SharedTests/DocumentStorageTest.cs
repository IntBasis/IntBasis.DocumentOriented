﻿using System.Dynamic;

#if MONGO_DB
namespace IntBasis.DocumentOriented.MongoDB.Tests;
#else
namespace IntBasis.DocumentOriented.RavenDB.Tests;
#endif

public class Category : IDocumentEntity
{
    public string? Id { get; set; }
    public string? Name { get; set; }

    public Category(string name)
    {
        Name = name;
    }

    internal Category(string? id, string name) : this(name)
    {
        Id = id;
    }
}

public class DocumentStorageTest
{
    [Theory(DisplayName = "Store/Retrieve auto-generated ID"), Integration]
    public async Task StoreAutoId(IDocumentStorage subject)
    {
        const string name = "Expected Name";
        var entity = new Category(name);

        await subject.Store(entity);
        entity.Id.Should().NotBeNullOrEmpty();

        var retrieved = await subject.Retrieve<Category>(entity.Id!);
        retrieved.Should().BeEquivalentTo(entity);
    }

    [Theory(DisplayName = "Store w/ given ID"), Integration]
    public async Task StoreId(IDocumentStorage subject)
    {
        var id = Guid.NewGuid().ToString();
        const string name = "My own ID test";
        var stored = new Category(id, name);

        await subject.Store(stored);
        stored.Id.Should().Be(id);

        var retrieved = await subject.Retrieve<Category>(id);
        retrieved.Should().BeEquivalentTo(stored);
    }

    [Theory(DisplayName = "Retrieve Missing returns null"), Integration]
    public async Task Missing(IDocumentStorage subject)
    {
        var id = Guid.NewGuid().ToString();

        var retrieved = await subject.Retrieve<Category>(id);

        retrieved.Should().BeNull();
    }

    class HasDynamic : IDocumentEntity
    {
        public string? Id { get; set; }
        public dynamic Metadata { get; set; } = new ExpandoObject();
    }

    [Theory(DisplayName = "Store Dynamic"), Integration]
    public async Task StoreDynamic(IDocumentStorage subject)
    {
        var entity = new HasDynamic
        {
            Metadata = new
            {
                Location = "Houston"
            }
        };
        Assert.Equal("Houston", entity.Metadata.Location);

        await subject.Store(entity);
        entity.Id.Should().NotBeNull();
        var retrieved = await subject.Retrieve<HasDynamic>(entity.Id!);

        retrieved!.Should().NotBeNull();
        Assert.NotNull(retrieved!.Metadata);
        Assert.Equal("Houston", retrieved.Metadata.Location);

        retrieved.Metadata.Value = 42;
        await subject.Store(retrieved);
        var retrieved2 = await subject.Retrieve<HasDynamic>(entity.Id!);

        retrieved2!.Should().NotBeNull();
        Assert.NotNull(retrieved2!.Metadata);
        Assert.Equal("Houston", retrieved2.Metadata.Location);
        Assert.Equal(42, retrieved2.Metadata.Value);
    }
}
