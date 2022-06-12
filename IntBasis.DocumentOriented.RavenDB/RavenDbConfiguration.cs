using System;

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

    public RavenDbConfiguration(string databaseName, params string[] serverUrls)
    {
        DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        ServerUrls = serverUrls ?? throw new ArgumentNullException(nameof(serverUrls));
    }
}
