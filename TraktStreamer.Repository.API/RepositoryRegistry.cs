using Spring.Context.Support;

namespace TraktStreamer.Repository.API
{
    public class RepositoryRegistry
    {
        #region Singleton

        private static RepositoryRegistry _instance;

        public static RepositoryRegistry Instance => _instance ?? (_instance = new RepositoryRegistry());

        #endregion // Singleton

        public IAuthorizationInfoRepository AuthorizationInfoRepository => ContextRegistry.GetContext().GetObject<IAuthorizationInfoRepository>();
    }
}
