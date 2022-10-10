using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Constants
{
    public class UrlGenerationConstants
    {
        public const string GENERIC_DOMAIN = "https://mfc-urlshortener-domain.com/"; //I would get this information from db in real application. Preferabably store in secret
        public const int SHORT_URL_LENGTH = 6; //I would get this information from db in real application. Preferabably store in secret
        public const int HASH_TRY_COUNT = 10; //I would make this value parametric. So that can be increased at any time
        public const string POSSIBLE_ITEMS_CHARSET = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
    }
}
