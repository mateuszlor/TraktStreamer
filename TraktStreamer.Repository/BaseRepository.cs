using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
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

        public T GetLast()
        {
            return DataService.Set<T>().OrderBy(x => x.ID).FirstOrDefault();
        }

        public long Save(T item)
        {
            DataService.Set<T>().AddOrUpdate(item);
            DataService.SaveChanges();
            return item.ID;
        }
    }
}
