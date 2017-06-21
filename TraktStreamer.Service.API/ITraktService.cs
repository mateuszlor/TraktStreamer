using System;
using System.Threading.Tasks;
using TraktApiSharp;

namespace TraktStreamer.Service.API
{
    public interface ITraktService
    {
        Task<TraktClient> GetAuthorizedTraktClientAsync(Func<string, string> userInputCallback);
    }
}
