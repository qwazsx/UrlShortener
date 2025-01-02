using Entity;
using DataAccess.Interface;
using Operation.Interface;
using Entity.Constants;

namespace Operation.Operation
{
    public class UrlShortenerOperation : IUrlShortenerOperation
    {
        private readonly IUrlGenerationOperation _urlGenerationOperation;
        private readonly IUrlShortenerRepository _urlShortenerRepository;

        public UrlShortenerOperation(IUrlGenerationOperation urlGenerationOperation, IUrlShortenerRepository urlShortenerRepository)
        {
            _urlGenerationOperation = urlGenerationOperation;
            _urlShortenerRepository = urlShortenerRepository;
        }

        public string CreateShortenedUrl(string url, string specifiedShortLink)
        {
            ValidateCreateShortenedUrlRequest(url, specifiedShortLink);

            if (!string.IsNullOrWhiteSpace(specifiedShortLink))
            {
                if (_urlShortenerRepository.GetUrlInfoByShortUrl(specifiedShortLink) != null)
                {
                    throw new Exception("Cannot generate link with this specified URL prefix");
                }

                _urlShortenerRepository.AddUrlInfo(new UrlInfo
                {
                    LongUrl = url,
                    ShortUrl = specifiedShortLink,
                    IsShortUrlSpecified = true
                });

                return $"{UrlGenerationConstants.GENERIC_DOMAIN}{specifiedShortLink}";
            }
            else
            {
                return GenerateAndSaveShortUrl(url);
            }
        }

        public string GetUrlByShortenedUrl(string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                throw new Exception("Invalid URL tag");
            }

            var urlInfo = _urlShortenerRepository.GetUrlInfoByShortUrl(shortUrl);

            if (urlInfo == null || string.IsNullOrWhiteSpace(urlInfo.LongUrl))
            {
                throw new Exception("Link with this URL does not exist");
            }

            if (!_urlGenerationOperation.IsValidUrl(urlInfo.LongUrl))
            {
                throw new Exception("Invalid URL");
            }

            return urlInfo.LongUrl;
        }

        private void ValidateCreateShortenedUrlRequest(string url, string specifiedShortLink)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("Empty URL");
            }

            if (!_urlGenerationOperation.IsValidUrl(url))
            {
                throw new Exception("Invalid URL");
            }

            if (!string.IsNullOrWhiteSpace(specifiedShortLink))
            {
                if (specifiedShortLink.Length != UrlGenerationConstants.SHORT_URL_LENGTH)
                {
                    throw new Exception("Invalid specified short link length");
                }

                if (!_urlGenerationOperation.IsValidShortUrl(specifiedShortLink))
                {
                    throw new Exception("Invalid specified short link");
                }
            }
        }

        private string GenerateAndSaveShortUrl(string url)
        {
            int tryCount = 0;
            string shortenedUrl;

            do
            {
                shortenedUrl = _urlGenerationOperation.GenerateShortUrl();

                if (_urlShortenerRepository.GetUrlInfoByShortUrl(shortenedUrl) != null)
                {
                    tryCount++;
                    continue;
                }

                _urlShortenerRepository.AddUrlInfo(new UrlInfo
                {
                    LongUrl = url,
                    ShortUrl = shortenedUrl
                });

                return $"{UrlGenerationConstants.GENERIC_DOMAIN}{shortenedUrl}";
            } while (tryCount < UrlGenerationConstants.HASH_TRY_COUNT);

            throw new Exception("Max try count has been reached");
        }
    }
}
