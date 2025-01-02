using Entity.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Operation.Operation
{
    public class UrlGenerationOperation : IUrlGenerationOperation
    {
        public bool IsValidUrl(string longUrl)
        {
            bool validationResult = Uri.TryCreate(longUrl, UriKind.Absolute, out Uri uriResult) &&
                                    uriResult != null &&
                                    (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return validationResult;
        }

        public bool IsValidShortUrl(string shortUrl)
        {
            HashSet<char> possibleItemsSet = new HashSet<char>(UrlGenerationConstants.POSSIBLE_ITEMS_CHARSET);
            return shortUrl.All(possibleItemsSet.Contains);
        }

        public string GenerateShortUrl()
        {
            StringBuilder shortUrl = new StringBuilder();
            Random random = new Random(GetNewSeed());
            HashSet<char> usedCharacters = new HashSet<char>();

            while (shortUrl.Length < UrlGenerationConstants.SHORT_URL_LENGTH)
            {
                char possibleItem = UrlGenerationConstants.POSSIBLE_ITEMS_CHARSET[random.Next(0, UrlGenerationConstants.POSSIBLE_ITEMS_CHARSET.Length)];
                if (usedCharacters.Add(possibleItem))
                {
                    shortUrl.Append(possibleItem);
                }
            }
            return shortUrl.ToString();
        }

        private int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            RandomNumberGenerator.Fill(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }
    }
}
