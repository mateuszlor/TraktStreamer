using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using ThePirateBay;
using TraktApiSharp;
using TraktApiSharp.Authentication;
using TraktStreamer.DAO.Model;
using TraktStreamer.Model.Enum;
using TraktStreamer.Repository;
using TraktStreamer.Service.API;

namespace TraktStreamer.ConsoleApp
{
    class Program
    {
        private static ILog _logger = LogManager.GetLogger(typeof(Program));

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

            var watchlist = await client.Sync.GetWatchedShowsAsync();

            foreach (var i in watchlist)
            {
                var progress = await client.Shows.GetShowWatchedProgressAsync(i.Show.Ids.Slug);

                if (progress.Aired > progress.Completed)
                {
                    var name = $"{i.Show.Title} S{progress.NextEpisode.SeasonNumber:00}E{progress.NextEpisode.Number:00}";
                    Console.WriteLine();
                    Console.WriteLine(name);

                    var torrents = tpbService.Search(name, TorrentResolutionEnum._720p);
                    var torrent = torrents.FirstOrDefault();

                    if (torrent != null)
                    {
                        _logger.Warn($"{torrent.Name} - S.{torrent.Seeds} L.{torrent.Leechers} - {torrent.Size} - {torrent.Magnet}");
                    }
                }
            }
            Console.WriteLine("\nTHAT'S ALL");
        }

        private static string HandleTraktCallback(string url)
        {
            System.Diagnostics.Process.Start(url);
            return Console.ReadLine();
        }
    }
}
