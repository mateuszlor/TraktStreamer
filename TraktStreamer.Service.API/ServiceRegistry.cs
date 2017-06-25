using Spring.Context.Support;

namespace TraktStreamer.Service.API
{
    public class ServiceRegistry
    {
        private static ServiceRegistry _instance;

        public static ServiceRegistry Instance => _instance ?? (_instance = new ServiceRegistry());

        public ITraktService TraktService = ContextRegistry.GetContext().GetObject<ITraktService>();
        public IThePirateBayService ThePirateBayService = ContextRegistry.GetContext().GetObject<IThePirateBayService>();
    }
}
