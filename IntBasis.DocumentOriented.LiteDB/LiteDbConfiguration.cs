namespace IntBasis.DocumentOriented.LiteDB;

public class LiteDbConfiguration
{
    public string FileName { get; set; }

    public LiteDbConfiguration(string fileName)
    {
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
    }
}
