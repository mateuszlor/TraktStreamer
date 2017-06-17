using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            MainAsync();
            
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
                var progressTask = client.Shows.GetShowWatchedProgressAsync(i.Show.Ids.Slug);
                progressTask.Wait();
                var progress = progressTask.Result;

                if (progress.Aired > progress.Completed)
                {
                    Console.WriteLine("{0} - {1} | {2}", i.Show.Title, progress.NextEpisode.Title, progress.NextEpisode.FirstAired);
                }
            }
        }
    }
}
