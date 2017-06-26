using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktStreamer.DAO.Model
{
    public class Series : BaseModel
    {
        public string Name { get; set; }
        public string TraktSlug { get; set; }
        public bool ToDownload { get; set; }
        public bool HasCustomLimits { get; set; }
        public long? Limit1080 { get; set; }
        public long? Limit720 { get; set; }
    }
}
