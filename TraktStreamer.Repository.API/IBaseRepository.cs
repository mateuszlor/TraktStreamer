using System.Collections.Generic;

namespace TraktStreamer.Repository.API
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        T GetLast();
        long Save(T item);
    }
}
