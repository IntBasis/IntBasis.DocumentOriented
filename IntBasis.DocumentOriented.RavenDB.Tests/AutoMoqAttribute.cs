using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

public class AutoMoqAttribute : AutoDataAttribute
{
    public AutoMoqAttribute()
        : base(BuildFixture)
    {
    }

    public static IFixture BuildFixture()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        return fixture;
    }
}
