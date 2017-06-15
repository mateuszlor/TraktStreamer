using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktApiSharp;
using TraktApiSharp.Authentication;

namespace TraktStreamer.ConsoleApp
{
    class Program
    {
        private static string CLIENT_ID = "1b7b3c21bf6079bd427eeede60f56d2ad65a467eebce117c0646a2bfa149eb7c";
        private static string CLIENT_SECRET = "15ca85809b45b81d5d1c35f8af58aa2e9a37d44b922bf77e9ef5a4670eefad56";

        static void Main(string[] args)
        {
            var client = new TraktClient(CLIENT_ID, CLIENT_SECRET);
            var authorizationUrl = client.OAuth.CreateAuthorizationUrl();
            System.Diagnostics.Process.Start(authorizationUrl);

            var pin = Console.ReadLine();

            var authorizationTask = client.OAuth.GetAuthorizationAsync(pin);
            authorizationTask.Wait();

            var autohrization = authorizationTask.Result;

            Console.WriteLine("{0} | {1} | {2}", client.Authorization.TokenType, client.Authorization.AccessToken, client.Authorization.RefreshToken);

            var watchlistTask = client.Sync.GetWatchedShowsAsync();
            watchlistTask.Wait();

            var watchlist = watchlistTask.Result;
            foreach (var i in watchlist)
            {
                var progressTask = client.Shows.GetShowWatchedProgressAsync(i.Show.Ids.Slug);
                progressTask.Wait();
                var progress = progressTask.Result;

                if (progress.Aired > progress.Completed)
                {
                    Console.WriteLine("{0} - {1} | {2}", i.Show.Title, progress.NextEpisode.Title, progress.NextEpisode.FirstAired);
                }
            }

            Console.WriteLine(new string('=', 50));
            Console.ReadKey();
        }
    }
}
