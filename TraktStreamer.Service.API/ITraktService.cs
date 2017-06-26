using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraktApiSharp;
using TraktStreamer.Model;

namespace TraktStreamer.Service.API
{
    public interface ITraktService
    {
        /// <summary>
        /// TraktClient getter with authorization callback
        /// </summary>
        /// <param name="userInputCallback">User inpur authorization callback (takes URL to Trakt.TV, returns PIN)</param>
        /// <returns></returns>
        Task<TraktClient> GetAuthorizedTraktClientAsync(Func<string, string> userInputCallback);

        Task<ICollection<EpizodeModel>> GetAllEpizodesToWatchAsync(TraktClient client);
    }
}
