using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring.Context.Support;

namespace TraktStreamer.Service.API
{
    public class ServiceRegistry
    {
        private static ServiceRegistry _instance;

        public static ServiceRegistry Instance => _instance ?? (_instance = new ServiceRegistry());

        public ITraktService TraktService = ContextRegistry.GetContext().GetObject<ITraktService>("traktService");
    }
}
