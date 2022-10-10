namespace Operation.Interface
{
    public interface IUrlShortenerOperation
    {
        string CreateShortenedUrl(string url, string specifiedShortLink);
        string GetUrlByShortenedUrl(string shortUrl);
    }
}