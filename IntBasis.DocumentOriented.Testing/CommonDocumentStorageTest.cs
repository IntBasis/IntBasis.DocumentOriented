using System.Dynamic;
using System.Threading.Tasks;
using FluentAssertions;

namespace IntBasis.DocumentOriented.Testing;

public class HasDynamic : IDocumentEntity
{
    public string? Id { get; set; }
    public dynamic Metadata { get; set; } = new ExpandoObject();
}

public static class CommonDocumentStorageTest
{
    public static async Task VerifyDynamicObjectStorage(IDocumentStorage subject)
    {
        var entity = new HasDynamic
        {
            Metadata = new
            {
                Location = "Houston"
            }
        };
        // entity.Metadata.Value = 42;
        ((string)entity.Metadata.Location).Should().Be("Houston");

        await subject.Store(entity);
        entity.Id.Should().NotBeNull();
        var retrieved = await subject.Retrieve<HasDynamic>(entity.Id!);

        retrieved!.Should().NotBeNull();
        ((object)retrieved!.Metadata)!.Should().NotBeNull();
        ((string?)retrieved.Metadata.Location).Should().Be("Houston");

        retrieved.Metadata.Value = 42;
        await subject.Store(retrieved);
        var retrieved2 = await subject.Retrieve<HasDynamic>(entity.Id!);

        retrieved2!.Should().NotBeNull();
        ((object)retrieved2!.Metadata)!.Should().NotBeNull();
        ((string?)retrieved2.Metadata?.Location).Should().Be("Houston");
        ((int?)retrieved2.Metadata?.Value).Should().Be(42);
    }
}
