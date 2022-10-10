using Entity;
using DataAccess.Interface;
using Operation.Interface;
using Entity.Constants;

namespace Operation.Operation
{
    public class UrlShortenerOperation : IUrlShortenerOperation
    {
        private readonly IUrlGenerationOperation urlGenerationOperation;
        private readonly IUrlShortenerRepository urlShortenerRepository;

        public UrlShortenerOperation(IUrlGenerationOperation urlGenerationOperation,IUrlShortenerRepository urlShortenerRepository)
        {
            this.urlGenerationOperation = urlGenerationOperation;
            this.urlShortenerRepository = urlShortenerRepository;
        }

        public string CreateShortenedUrl(string url, string specifiedShortLink)
        {
            ValidateCreateShortenedUrlRequest(url, specifiedShortLink);

            if (!string.IsNullOrWhiteSpace(specifiedShortLink))
            {
                UrlInfo urlInfo = urlShortenerRepository.GetUrlInfoByShortUrl(specifiedShortLink);
                if (urlInfo != null)
                {
                    throw new Exception("Cannot generate link with this specified url prefix");
                }
                urlShortenerRepository.AddUrlInfo(new UrlInfo { LongUrl = url, ShortUrl = specifiedShortLink, IsShortUrlSpecified = true });
                return string.Concat(UrlGenerationConstants.GENERIC_DOMAIN, specifiedShortLink);
            }
            else
            {
                int tryCount = 0;
                string shortenedUrl = string.Empty;
                do
                {
                    shortenedUrl = urlGenerationOperation.GenerateShortUrl();
                    UrlInfo urlInfo = urlShortenerRepository.GetUrlInfoByShortUrl(shortenedUrl);
                    if (urlInfo != null)
                    {
                        tryCount++;
                        continue;
                    }

                    urlShortenerRepository.AddUrlInfo(new UrlInfo { LongUrl = url, ShortUrl = shortenedUrl });
                    return string.Concat(UrlGenerationConstants.GENERIC_DOMAIN, shortenedUrl);
                    //I would do in try/catch statement if I were use sql db. And check for (ex is DuplicateKeyException) for trying again
                    //And for larger scale product I would use range definitions for hashing
                } while (tryCount < UrlGenerationConstants.HASH_TRY_COUNT);
            }
            throw new Exception("Max try count has been reached");
        }

        public string GetUrlByShortenedUrl(string shortUrl)
        {
            if (shortUrl == null || shortUrl.Length != UrlGenerationConstants.SHORT_URL_LENGTH)
            {
                throw new Exception("Invalid url tag");
            }

            UrlInfo urlInfo = urlShortenerRepository.GetUrlInfoByShortUrl(shortUrl);

            if (urlInfo == null || string.IsNullOrWhiteSpace(urlInfo.LongUrl))
            {
                throw new Exception("Link with this url does not exists");
            }
            if (!urlGenerationOperation.IsValidUrl(urlInfo.LongUrl))
            {
                throw new Exception("Invalid url");
            }
            return urlInfo.LongUrl;
        }

        private void ValidateCreateShortenedUrlRequest(string url, string specifiedShortLink)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("Empty url");
            }
            if (!urlGenerationOperation.IsValidUrl(url))
            {
                throw new Exception("Invalid url");
            }
            if (!string.IsNullOrWhiteSpace(specifiedShortLink) && specifiedShortLink.Length != UrlGenerationConstants.SHORT_URL_LENGTH)
            {
                throw new Exception("Invalid specified short link length");
            }
            if (!string.IsNullOrWhiteSpace(specifiedShortLink) && !urlGenerationOperation.IsValidShortUrl(specifiedShortLink))
            {
                throw new Exception("Invalid specified short link");
            }
        }

    }
}
