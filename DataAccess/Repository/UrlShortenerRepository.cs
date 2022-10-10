using DataAccess.Interface;
using Entity;

namespace DataAccess.Repository
{
    public class UrlShortenerRepository : IUrlShortenerRepository
    {
        private List<UrlInfo> urlInfos;
        public readonly ApiContext context;

        public UrlShortenerRepository()
        {
            //add some default urlInfos
            urlInfos = new List<UrlInfo>()
            {
                new UrlInfo
                {
                    RecordId = 1,
                    InsertDate = DateTime.ParseExact("2022-07-08 13:40", "yyyy-MM-dd HH:mm",System.Globalization.CultureInfo.InvariantCulture),
                    LongUrl = "https://github.com/qwazsx",
                    ShortUrl = "GITHB1"
                },
                new UrlInfo
                {
                    RecordId = 2,
                    InsertDate = DateTime.ParseExact("2022-07-09 14:40", "yyyy-MM-dd HH:mm",System.Globalization.CultureInfo.InvariantCulture),
                    LongUrl = "https://www.linkedin.com/in/muhammed-fatih-cambek/",
                    ShortUrl = "LNKDN2"
                },
                new UrlInfo
                {
                    RecordId = 3,
                    InsertDate = DateTime.ParseExact("2022-07-10 15:40", "yyyy-MM-dd HH:mm",System.Globalization.CultureInfo.InvariantCulture),
                    LongUrl = "https://www.mercedesbenzturk.com.tr/Kariyer/Is-Imkanlari",
                    ShortUrl = "MRSD3S",
                    IsShortUrlSpecified = true
                },
            };
            context = new ApiContext();
            if (context.UrlInfos.Count() == 0)
            {
                context.UrlInfos.AddRange(urlInfos);
            }
            context.SaveChanges();
        }


        public List<UrlInfo> GetUrlInfos()
        {
            return context.UrlInfos.ToList();
        }

        public UrlInfo GetUrlInfoByRecordId(int recordId)
        {
            return context.UrlInfos?.FirstOrDefault(x => x.RecordId == recordId); //Expected default unique index
        }

        public UrlInfo GetUrlInfoByShortUrl(string shortUrl)
        {
            return context.UrlInfos?.FirstOrDefault(x => x.ShortUrl == shortUrl); //Index for lookups
        }

        public void AddUrlInfo(UrlInfo urlInfo)
        {
            if (context.UrlInfos.Any(x => x.ShortUrl == urlInfo.ShortUrl)) throw new Exception("Link with given url already registered"); //Db level dupplication prevention mockup

            if (context.UrlInfos.Any())
            {
                var maxId = context.UrlInfos.Max(x => x.RecordId);
                urlInfo.RecordId = maxId + 1;
            }
            else
            {
                urlInfo.RecordId = 1;
            }
            urlInfo.InsertDate = DateTime.Now;
            context.UrlInfos.AddRange(urlInfo);
            context.SaveChanges();
        }

        public void UpdateUrlInfo(UrlInfo urlInfo)
        {
            var UrlInfoToUpdate = GetUrlInfoByRecordId(urlInfo.RecordId);
            if (UrlInfoToUpdate != null)
            {
                UrlInfoToUpdate.LongUrl = urlInfo.LongUrl;
                UrlInfoToUpdate.ShortUrl = urlInfo.ShortUrl;
                context.UpdateRange(UrlInfoToUpdate);
            }
            context.SaveChanges();
        }
    }
}
