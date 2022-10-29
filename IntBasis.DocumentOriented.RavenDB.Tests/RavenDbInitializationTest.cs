using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace IntBasis.DocumentOriented.RavenDB.Tests;

public class RavenDbInitializationTest
{
    [Theory(DisplayName = "RavenDB Store"), Integration]
    public async Task CreatesDb(IDocumentStore store)
    {
        // Setup: Delete DB if it already exists.
        // It is OK to Delete a DB that does not exist.
        const string DatabaseName = "TempInitTest";
        var deleteOperation = new DeleteDatabasesOperation(DatabaseName, hardDelete: true, fromNode: null, timeToWaitForConfirmation: null);
        store.Maintenance.Server.Send<DeleteDatabaseResult>(deleteOperation);

        var config = new RavenDbConfiguration(DatabaseName, IntegrationAttribute.TestServer);
        var store2 = RavenDbInitialization.InitializeDocumentStore(config);
        var session = store2.OpenAsyncSession();
        var documentStorage = new RavenDbDocumentStorage(session);
        var item = new Product { Name = "First record in new DB" };
        await documentStorage.Store(item);

        item.Id.Should().Be("products/1-A", "corresponding to the first Product document in the DB");
    }
}
