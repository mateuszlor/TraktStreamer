using Spring.Context.Support;
using TraktStreamer.Repository.API;

namespace TraktStreamer.Repository
{
    public class RepositoryRegistry
    {
        #region Singleton

        private static RepositoryRegistry _instance;

        public static RepositoryRegistry Instance => _instance ?? (_instance = new RepositoryRegistry());

        #endregion // Singleton

        public IAuthorizationInfoRepository AuthorizationInfoRepository => ContextRegistry.GetContext().GetObject<IAuthorizationInfoRepository>("authorizationInfoRepository");
    }
}
