using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktStreamer.DAO.Model
{
    public class Series : BaseModel
    {
        public string TraktSlug { get; set; }
        public bool ToDownload { get; set; }
    }
}
