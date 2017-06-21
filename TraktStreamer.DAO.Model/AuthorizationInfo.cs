using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktStreamer.DAO.Model
{
    public class AuthorizationInfo : BaseModel
    {
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUsageDate { get; set; } = DateTime.Now;
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
