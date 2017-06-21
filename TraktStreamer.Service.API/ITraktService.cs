using TraktApiSharp;

namespace TraktStreamer.Service.API
{
    public interface ITraktService
    {
        TraktClient GetAuthorizedTraktClientAsync();
    }
}
