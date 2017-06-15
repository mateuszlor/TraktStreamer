using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktStreamer.DAO.Model;

namespace TraktStreamer.DAO.DataService
{
    public class DataService : DbContext
    {
        public DbSet<Series> Series { get; }
        public DbSet<AuthorizationInfo> AuthorizationInfo { get; }
    }
}
