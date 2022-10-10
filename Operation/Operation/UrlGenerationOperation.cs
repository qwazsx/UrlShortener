using Entity.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operation.Operation
{
    public class UrlGenerationOperation : IUrlGenerationOperation
    {
        public bool IsValidUrl(string longUrl)
        {
            bool validationResult = Uri.TryCreate(longUrl, UriKind.Absolute, out Uri uriResult) && uriResult != null && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return validationResult;
        }

        public bool IsValidShortUrl(string shortUrl)
        {
            for (int i = 0; i < shortUrl.Length; i++)
            {
                char current = shortUrl[i];
                if (!UrlGenerationConstants.POSSIBLE_ITEMS_CHARSET.Contains(current)) { return false; } 
            }
            return true;
        }

        public string GenerateShortUrl()
        {
            string shortUrl = string.Empty;
            Random random = new Random(GetNewSeed());
            while (shortUrl.Length < UrlGenerationConstants.SHORT_URL_LENGTH)
            {
                string possibleItem = UrlGenerationConstants.POSSIBLE_ITEMS_CHARSET[random.Next(0, UrlGenerationConstants.POSSIBLE_ITEMS_CHARSET.Length)].ToString();
                if (!shortUrl.Contains(possibleItem, StringComparison.CurrentCulture)) shortUrl += possibleItem;
            }
            return shortUrl;

        }
        private int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }
    }
}
