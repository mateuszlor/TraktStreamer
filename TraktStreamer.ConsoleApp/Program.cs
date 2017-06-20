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

namespace TraktStreamer.ConsoleApp
{
    class Program
    {
        private static string CLIENT_ID = "1b7b3c21bf6079bd427eeede60f56d2ad65a467eebce117c0646a2bfa149eb7c";
        private static string CLIENT_SECRET = "15ca85809b45b81d5d1c35f8af58aa2e9a37d44b922bf77e9ef5a4670eefad56";

        static void Main(string[] args)
        {
            var exec = MainAsync();
            //exec.Wait();
            
            Console.ReadKey();

            Console.WriteLine(exec.Exception?.ToString() ?? "NO EXCEPTIONS");

            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            var client = new TraktClient(CLIENT_ID, CLIENT_SECRET);

            var repo = RepositoryRegistry.Instance.AuthorizationInfoRepository;
            var auth = repo.GetAll().LastOrDefault();

            if (auth is null)
            {
                var authorizationUrl = client.OAuth.CreateAuthorizationUrl();
                System.Diagnostics.Process.Start(authorizationUrl);

                var pin = Console.ReadLine();

                var authorization = await client.OAuth.GetAuthorizationAsync(pin);
                
                auth = new AuthorizationInfo
                {
                    AccessToken = authorization.AccessToken,
                    RefreshToken = authorization.RefreshToken
                };

                repo.Save(auth);
            }
            else
            {
                var authorization = TraktAuthorization.CreateWith(auth.AccessToken, auth.RefreshToken);
                client.Authorization = authorization;
            }

            Console.WriteLine("{0} | {1} | {2}", client.Authorization.TokenType, client.Authorization.AccessToken, client.Authorization.RefreshToken);

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
