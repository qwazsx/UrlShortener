using DataAccess.Interface;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class UrlShortenerRepository : IUrlShortenerRepository
    {
        private readonly ApiContext _context;

        public UrlShortenerRepository(ApiContext context)
        {
            _context = context;

            // Add some default urlInfos if the database is empty
            if (!_context.UrlInfos.Any())
            {
                var urlInfos = new List<UrlInfo>
                {
                    new UrlInfo
                    {
                        RecordId = 1,
                        InsertDate = DateTime.ParseExact("2022-07-08 13:40", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                        LongUrl = "https://github.com/qwazsx",
                        ShortUrl = "GITHB1"
                    },
                    new UrlInfo
                    {
                        RecordId = 2,
                        InsertDate = DateTime.ParseExact("2022-07-09 14:40", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                        LongUrl = "https://www.linkedin.com/in/muhammed-fatih-cambek/",
                        ShortUrl = "LNKDN2"
                    },
                    new UrlInfo
                    {
                        RecordId = 3,
                        InsertDate = DateTime.ParseExact("2022-07-10 15:40", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                        LongUrl = "https://www.mercedesbenzturk.com.tr/Kariyer/Is-Imkanlari",
                        ShortUrl = "MRSD3S",
                        IsShortUrlSpecified = true
                    }
                };
                _context.UrlInfos.AddRange(urlInfos);
                _context.SaveChanges();
            }
        }

        public List<UrlInfo> GetUrlInfos()
        {
            return _context.UrlInfos.ToList();
        }

        public UrlInfo GetUrlInfoByRecordId(int recordId)
        {
            return _context.UrlInfos.FirstOrDefault(x => x.RecordId == recordId);
        }

        public UrlInfo GetUrlInfoByShortUrl(string shortUrl)
        {
            return _context.UrlInfos.FirstOrDefault(x => x.ShortUrl == shortUrl);
        }

        public void AddUrlInfo(UrlInfo urlInfo)
        {
            if (_context.UrlInfos.Any(x => x.ShortUrl == urlInfo.ShortUrl))
                throw new Exception("Link with given url already registered");

            if (_context.UrlInfos.Any())
            {
                var maxId = _context.UrlInfos.Max(x => x.RecordId);
                urlInfo.RecordId = maxId + 1;
            }
            else
            {
                urlInfo.RecordId = 1;
            }
            urlInfo.InsertDate = DateTime.Now;
            _context.UrlInfos.Add(urlInfo);
            _context.SaveChanges();
        }

        public void UpdateUrlInfo(UrlInfo urlInfo)
        {
            var urlInfoToUpdate = GetUrlInfoByRecordId(urlInfo.RecordId);
            if (urlInfoToUpdate != null)
            {
                urlInfoToUpdate.LongUrl = urlInfo.LongUrl;
                urlInfoToUpdate.ShortUrl = urlInfo.ShortUrl;
                _context.Update(urlInfoToUpdate);
                _context.SaveChanges();
            }
        }
    }
}
