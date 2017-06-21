using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePirateBay;
using TraktApiSharp;
using TraktApiSharp.Authentication;
using TraktStreamer.DAO.Model;
using TraktStreamer.Repository;
using TraktStreamer.Service.API;

namespace TraktStreamer.ConsoleApp
{
    class Program
    {
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
            var service = ServiceRegistry.Instance.TraktService;
            var client = service.GetAuthorizedTraktClientAsync();

            var watchlist = await client.Sync.GetWatchedShowsAsync();

            foreach (var i in watchlist)
            {
                var progress = await client.Shows.GetShowWatchedProgressAsync(i.Show.Ids.Slug);

                if (progress.Aired > progress.Completed)
                {
                    var name = $"{i.Show.Title} S{progress.NextEpisode.SeasonNumber:00}E{progress.NextEpisode.Number:00}";
                    Console.WriteLine(name);
                    var torrents = Tpb.Search(new Query(name + " 1080p", 0, QueryOrder.BySize)).Where(t => t.Seeds > 0 && t.SizeBytes > 1 * 1024 * 1024 * 1024);
                    if (!torrents.Any())
                    {
                        torrents = Tpb.Search(new Query(name + " 720p", 0, QueryOrder.BySize))
                            .Where(t => t.Seeds > 0 && t.SizeBytes > 500 * 1024 * 1024);
                    }
                    //Console.WriteLine(string.Join(" | ", torrents.Select(t => $"{t.Name} - S.{t.Seeds} L.{t.Leechers} - {t.Size}")));
                    var torrent = torrents.FirstOrDefault();
                    if (torrent != null)
                    {
                        Console.WriteLine($"{torrent.Name} - S.{torrent.Seeds} L.{torrent.Leechers} - {torrent.Size} - {torrent.Magnet}");
                    }
                }
            }
        }
    }
}
