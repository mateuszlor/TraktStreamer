using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktStreamer.DAO.DataService;
using TraktStreamer.DAO.Model;

namespace TraktStreamer.Repository
{
    public abstract class BaseRepository<T> where T : BaseModel
    {
        public List<T> GetAll()
        {
            var ctx = new DataService();
            return ctx.Set<T>().ToList();
        }

        public long Save(T item)
        {
            var ctx = new DataService();
            var saved = ctx.Set<T>().Add(item);
            ctx.SaveChanges();
            return saved.ID;
        }
    }
}
