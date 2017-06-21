using System;
using System.Threading.Tasks;
using TraktApiSharp;
using TraktApiSharp.Authentication;
using TraktStreamer.DAO.Model;
using TraktStreamer.Repository.API;
using TraktStreamer.Service.API;

namespace TraktStreamer.Service
{
    public class TraktService : ITraktService
    {
        private string CLIENT_ID;
        private string CLIENT_SECRET;

        private IAuthorizationInfoRepository AuthorizationInfoRepository;

        /// <summary>
        /// TraktClient getter with authorization callback
        /// </summary>
        /// <param name="userInputCallback">User inpur authorization callback (takes URL to Trakt.TV, returns PIN)</param>
        /// <returns></returns>
        public async Task<TraktClient> GetAuthorizedTraktClientAsync(Func<string, string> userInputCallback)
        {
            var client = new TraktClient(CLIENT_ID, CLIENT_SECRET);

            var auth = AuthorizationInfoRepository.GetLast();

            if (auth is null)
            {
                var authorizationUrl = client.OAuth.CreateAuthorizationUrl();
                
                //System.Diagnostics.Process.Start(authorizationUrl);
                //var pin = Console.ReadLine();

                var pin = userInputCallback(authorizationUrl);

                var authorization = await client.OAuth.GetAuthorizationAsync(pin);

                auth = new AuthorizationInfo
                {
                    AccessToken = authorization.AccessToken,
                    RefreshToken = authorization.RefreshToken
                };

                AuthorizationInfoRepository.Save(auth);
            }
            else
            {
                var authorization = TraktAuthorization.CreateWith(auth.AccessToken, auth.RefreshToken);
                client.Authorization = authorization;
                
                auth.LastUsageDate = DateTime.Now;
                AuthorizationInfoRepository.Save(auth);
            }

            Console.WriteLine("{0} | {1} | {2}", client.Authorization.TokenType, client.Authorization.AccessToken, client.Authorization.RefreshToken);

            return client;
        }
    }
}
