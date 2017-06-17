using System.Collections.Generic;

namespace TraktStreamer.Repository.API
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        long Save(T item);
    }
}
