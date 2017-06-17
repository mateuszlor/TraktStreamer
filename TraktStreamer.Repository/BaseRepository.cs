using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktStreamer.Core;
using TraktStreamer.DAO.DataService;
using TraktStreamer.DAO.Model;
using TraktStreamer.Repository.API;

namespace TraktStreamer.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> 
        where T : BaseModel
    {
        public DataService DataService { get; set; }

        public List<T> GetAll()
        {
            return DataService.Set<T>().ToList();
        }

        public long Save(T item)
        {
            var saved = DataService.Set<T>().Add(item);
            DataService.SaveChanges();
            return saved.ID;
        }
    }
}
