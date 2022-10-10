using Entity;

namespace DataAccess.Interface
{
    public interface IUrlShortenerRepository
    {
        void AddUrlInfo(UrlInfo urlInfo);
        UrlInfo GetUrlInfoByRecordId(int recordId);
        UrlInfo GetUrlInfoByShortUrl(string shortUrl);
        List<UrlInfo> GetUrlInfos();
        void UpdateUrlInfo(UrlInfo urlInfo);
    }
}