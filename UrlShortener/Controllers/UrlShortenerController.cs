using Microsoft.AspNetCore.Mvc;
using Operation.Interface;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlShortenerController : Controller
    {
        private IUrlShortenerOperation urlShortenerOperation;
        private readonly ILogger<UrlShortenerController> logger;
        public UrlShortenerController(IUrlShortenerOperation urlShortenerOperation, ILogger<UrlShortenerController> logger)
        {
            this.urlShortenerOperation = urlShortenerOperation;
            this.logger = logger;
        }

        [HttpPost("CreateShortenedUrl")]
        public string CreateShortenedUrl(string url)
        {
            return urlShortenerOperation.CreateShortenedUrl(url, null);
        }

        [HttpPost("CreateShortenedUrlWithShortLink")]
        public string CreateShortenedUrlWithShortLink(string url, string specifiedShortLink)
        {
            
            return urlShortenerOperation.CreateShortenedUrl(url, specifiedShortLink);
        }

        [HttpPost("GetUrlByShortenedUrl")]
        public string GetUrlByShortenedUrl(string shortUrl)
        {
            return urlShortenerOperation.GetUrlByShortenedUrl(shortUrl);
        }

        [HttpPost("RedirectUrlByShortenedUrl")]
        public ActionResult RedirectUrlByShortenedUrl(string shortUrl)
        {
            return Redirect(urlShortenerOperation.GetUrlByShortenedUrl(shortUrl)); // Does not actually work because of CORS policy
        }
    }
}
