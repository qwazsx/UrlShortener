using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    [Serializable]
    public class UrlInfo
    {
        public UrlInfo()
        {

        }
        [Key]
        public int RecordId { get; set; }
        public DateTime InsertDate { get; set; }
        public string LongUrl { get; set; } = null;
        public string ShortUrl { get; set; } = null;
        public string SpecifiedShortUrl { get; set; } = null;
        public bool IsShortUrlSpecified { get; set; }
    }
}
