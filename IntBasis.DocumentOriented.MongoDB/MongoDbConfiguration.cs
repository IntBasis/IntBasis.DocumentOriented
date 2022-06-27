namespace IntBasis.DocumentOriented.MongoDB;

public class MongoDbConfiguration
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }

    public MongoDbConfiguration(string connectionString, string databaseName)
    {
        ConnectionString = connectionString;
        DatabaseName = databaseName;
    }
}
