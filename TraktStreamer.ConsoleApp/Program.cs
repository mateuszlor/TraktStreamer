using System;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using TraktStreamer.Model.Enum;
using TraktStreamer.Service.API;

namespace TraktStreamer.ConsoleApp
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var main = MainAsync();

            //main.Wait();

            Console.ReadKey();
            if (main.Exception != null)
            {
                throw new Exception("Exception occured in async call", main.Exception);
            }
            
            Console.WriteLine("NO EXCEPTIONS");
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            var traktService = ServiceRegistry.Instance.TraktService;
            var tpbService = ServiceRegistry.Instance.ThePirateBayService;

            var client = await traktService.GetAuthorizedTraktClientAsync(HandleTraktCallback);
            var watchlist = await traktService.GetAllEpizodesToWatchAsync(client);

            foreach (var i in watchlist)
            {
                Console.WriteLine();
                _logger.Info(i);

                var torrents = tpbService.Search(i.ToString(), TorrentResolutionEnum._720p);
                var torrent = torrents.FirstOrDefault();

                if (torrent != null)
                {
                    _logger.Warn($"{torrent.Name} - S.{torrent.Seeds} L.{torrent.Leechers} - {torrent.Size} - {torrent.Magnet}");
                }
            }
            _logger.Info("\nTHAT'S ALL");
        }

        private static string HandleTraktCallback(string url)
        {
            System.Diagnostics.Process.Start(url);
            return Console.ReadLine();
        }
    }
}
