using Spring.Context.Support;
using TraktStreamer.Repository.API;

namespace TraktStreamer.Repository
{
    public class RepositoryRegistry
    {
        #region Singleton

        public static RepositoryRegistry Instance => new RepositoryRegistry();

        #endregion // Singleton

        public IAuthorizationInfoRepository AuthorizationInfoRepository => ContextRegistry.GetContext().GetObject<IAuthorizationInfoRepository>("authorizationInfoRepository");
    }
}
