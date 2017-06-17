using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring.Context.Support;
using TraktStreamer.DAO.DataService;

namespace TraktStreamer.Core
{
    public static class ApplicationContext
    {
        public static DataService DataService => ContextRegistry.GetContext().GetObject<DataService>("dataService");
    }
}
