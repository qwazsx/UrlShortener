namespace Operation.Operation
{
    public interface IUrlGenerationOperation
    {
        string GenerateShortUrl();
        bool IsValidUrl(string longUrl);
        bool IsValidShortUrl(string shortUrl);
    }
}