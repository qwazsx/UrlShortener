using Xunit;
using NSubstitute;
using Operation.Operation;
using DataAccess.Interface;
using AutoFixture;
using Entity;
using Entity.Constants;

namespace UrlShortener.UnitTests
{
    public class UrlShortenerOperationTests
    {
        private readonly UrlShortenerOperation _urlShortenerOperation;
        private readonly IUrlGenerationOperation _urlGenerationOperation = Substitute.For<IUrlGenerationOperation>();
        private readonly IUrlShortenerRepository _urlShortenerRepository = Substitute.For<IUrlShortenerRepository>();
        private readonly IFixture _fixture = new Fixture();

        public UrlShortenerOperationTests()
        {
            _urlShortenerOperation = new UrlShortenerOperation(_urlGenerationOperation, _urlShortenerRepository);
        }

        [Fact]
        public void CreateShortenedUrl_ShouldCreateUrlItem_WhenAllParametersAreValid()
        {
            // Arrange
            const string longUrl = "https://www.valid-test-url.com.tr";
            const string shortUrl = "SHORTU";

            _urlGenerationOperation.IsValidUrl(longUrl).Returns(true);
            _urlGenerationOperation.GenerateShortUrl().Returns(shortUrl);
            _urlShortenerRepository.GetUrlInfoByShortUrl(shortUrl).Returns((UrlInfo)null);

            // Act
            var result = _urlShortenerOperation.CreateShortenedUrl(longUrl, null);

            // Assert
            _urlGenerationOperation.Received(1).IsValidUrl(longUrl);
            _urlGenerationOperation.Received(1).GenerateShortUrl();
            _urlShortenerRepository.Received(1).GetUrlInfoByShortUrl(shortUrl);
            //_urlShortenerRepository.Received(1).AddUrlInfo(Arg.Is<UrlInfo>(u =>
            //    u.LongUrl == longUrl &&
            //    u.ShortUrl == shortUrl &&
            //    u.InsertDate != default(DateTime) &&
            //    u.RecordId > 0));
            _urlShortenerRepository.Received(1).AddUrlInfo(Arg.Any<UrlInfo>());
            Assert.Equal($"{UrlGenerationConstants.GENERIC_DOMAIN}{shortUrl}", result);
        }

        [Fact]
        public void CreateShortenedUrl_ShouldCreateUrlItem_WhenUniqueShortLinkSpecified()
        {
            // Arrange
            const string longUrl = "https://www.valid-test-url.com.tr";
            const string shortUrl = "SHORTU";

            _urlGenerationOperation.IsValidUrl(longUrl).Returns(true);
            _urlGenerationOperation.IsValidShortUrl(shortUrl).Returns(true);
            _urlShortenerRepository.GetUrlInfoByShortUrl(shortUrl).Returns((UrlInfo)null);

            // Act
            var result = _urlShortenerOperation.CreateShortenedUrl(longUrl, shortUrl);

            // Assert
            _urlGenerationOperation.Received(1).IsValidUrl(longUrl);
            _urlGenerationOperation.Received(1).IsValidShortUrl(shortUrl);
            _urlGenerationOperation.DidNotReceive().GenerateShortUrl();
            _urlShortenerRepository.Received(1).GetUrlInfoByShortUrl(shortUrl);
            _urlShortenerRepository.Received(1).AddUrlInfo(Arg.Is<UrlInfo>(u => u.LongUrl == longUrl && u.ShortUrl == shortUrl && u.IsShortUrlSpecified));
            Assert.Equal($"{UrlGenerationConstants.GENERIC_DOMAIN}{shortUrl}", result);
        }

        [Theory]
        [InlineData(null, null, "Empty URL")]
        [InlineData("invalid-test-url.com.tr", null, "Invalid URL")]
        [InlineData("https://www.valid-test-url.com.tr", "SHORTUT", "Invalid specified short link length")]
        [InlineData("https://www.valid-test-url.com.tr", "SHORT?", "Invalid specified short link")]
        [InlineData("https://www.valid-test-url.com.tr", "SHORTX", "Cannot generate link with this specified URL prefix")]
        [InlineData("https://www.valid-test-url.com.tr", null, "Max try count has been reached")]
        public void CreateShortenedUrl_ShouldNotCreateUrlItem_WhenInputDetailsAreInvalid(string longUrl, string specifiedShortLink, string exceptionMessage)
        {
            // Arrange
            _urlGenerationOperation.IsValidUrl(null).Returns(false);
            _urlGenerationOperation.IsValidUrl(longUrl).Returns(longUrl != null && !longUrl.Contains("invalid"));
            _urlGenerationOperation.IsValidShortUrl("SHORT?").Returns(false);
            _urlGenerationOperation.IsValidShortUrl("SHORTX").Returns(true);

            _urlGenerationOperation.GenerateShortUrl().Returns("SHORTS");
            _urlShortenerRepository.GetUrlInfoByShortUrl("SHORTS").Returns(new UrlInfo());
            _urlShortenerRepository.GetUrlInfoByShortUrl("SHORTX").Returns(new UrlInfo());

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _urlShortenerOperation.CreateShortenedUrl(longUrl, specifiedShortLink));
            Assert.Equal(exceptionMessage, ex.Message);
        }

        [Fact]
        public void GetUrlByShortenedUrl_ShouldGetLongUrlItem_WhenAllParametersAreValid()
        {
            // Arrange
            const string shortUrl = "SHORTU";
            var urlInfo = _fixture.Build<UrlInfo>().With(c => c.ShortUrl, shortUrl).Create();

            _urlGenerationOperation.IsValidUrl(urlInfo.LongUrl).Returns(true);
            _urlShortenerRepository.GetUrlInfoByShortUrl(shortUrl).Returns(urlInfo);

            // Act
            var result = _urlShortenerOperation.GetUrlByShortenedUrl(shortUrl);

            // Assert
            Assert.Equal(urlInfo.LongUrl, result);
        }

        [Theory]
        [InlineData(null, "Invalid URL tag")]
        [InlineData("SHORTUT", "Link with this URL does not exist")]
        [InlineData("EXIRTL", "Link with this URL does not exist")]
        [InlineData("EXIRTW", "Link with this URL does not exist")]
        [InlineData("INVLDD", "Invalid URL")]
        public void GetUrlByShortenedUrl_ShouldNotGetLongUrlItem_WhenInputDetailsAreInvalid(string shortUrl, string exceptionMessage)
        {
            // Arrange
            var urlInfo = _fixture.Build<UrlInfo>().With(c => c.LongUrl, shortUrl).Create();
            urlInfo.LongUrl = null;

            _urlShortenerRepository.GetUrlInfoByShortUrl("EXIRTL").Returns(urlInfo);
            _urlShortenerRepository.GetUrlInfoByShortUrl("EXIRTW").Returns((UrlInfo)null);

            urlInfo = _fixture.Build<UrlInfo>().With(c => c.LongUrl, shortUrl).Create();
            _urlShortenerRepository.GetUrlInfoByShortUrl("INVLDD").Returns(urlInfo);
            _urlGenerationOperation.IsValidUrl("INVLDD").Returns(false);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _urlShortenerOperation.GetUrlByShortenedUrl(shortUrl));
            Assert.Equal(exceptionMessage, ex.Message);
        }
    }
}
