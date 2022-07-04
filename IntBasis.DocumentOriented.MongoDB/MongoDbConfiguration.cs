namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbConfiguration
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }

    // Configuration Binder requires parameterless constructor
    public MongoDbConfiguration() : this("", "")
    {
    }

    public MongoDbConfiguration(string connectionString, string databaseName)
    {
        ConnectionString = connectionString;
        DatabaseName = databaseName;
    }
}
