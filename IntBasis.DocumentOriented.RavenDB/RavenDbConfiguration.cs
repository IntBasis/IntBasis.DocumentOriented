namespace IntBasis.DocumentOriented.RavenDB;

public class RavenDbConfiguration
{
    /// <summary>
    /// Default database that DocumentStore will interact with
    /// </summary>
    public string DatabaseName { get; set; }

    /// <summary>
    /// URL to the Server, or list of URLs to all Cluster Servers (Nodes)
    /// </summary>
    public string[] ServerUrls { get; set; }

    /// <summary>
    /// By default, maximum number of requests that session can send to server is 30. This number, if everything is done correctly, should never be reached. Remote calls are expensive, and the number of remote calls per "session" should be as close to 1 as possible.
    /// <see href="https://ravendb.net/docs/article-page/5.4/csharp/client-api/session/configuration/how-to-change-maximum-number-of-requests-per-session"/>
    /// </summary>
    public int? MaxNumberOfRequestsPerSession { get; set; }

    // Configuration Binder requires parameterless constructor
    public RavenDbConfiguration()
    {
    }

    public RavenDbConfiguration(string databaseName, params string[] serverUrls)
    {
        DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        ServerUrls = serverUrls ?? throw new ArgumentNullException(nameof(serverUrls));
    }
}
