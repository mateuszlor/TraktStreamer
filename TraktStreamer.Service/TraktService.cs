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

        public TraktClient GetAuthorizedTraktClientAsync()
        {
            var client = new TraktClient(CLIENT_ID, CLIENT_SECRET);

            var auth = AuthorizationInfoRepository.GetLast();

            if (auth is null)
            {
                var authorizationUrl = client.OAuth.CreateAuthorizationUrl();
                System.Diagnostics.Process.Start(authorizationUrl);

                var pin = Console.ReadLine();

                var authorizationTask = client.OAuth.GetAuthorizationAsync(pin);

                authorizationTask.ContinueWith(SaveAuth, TaskContinuationOptions.OnlyOnRanToCompletion);

            }
            else
            {
                var authorization = TraktAuthorization.CreateWith(auth.AccessToken, auth.RefreshToken);
                client.Authorization = authorization;
            }

            Console.WriteLine("{0} | {1} | {2}", client.Authorization.TokenType, client.Authorization.AccessToken, client.Authorization.RefreshToken);

            return client;
        }

        private void SaveAuth(Task<TraktAuthorization> authorization)
        {
            var auth = new AuthorizationInfo
            {
                AccessToken = authorization.Result.AccessToken,
                RefreshToken = authorization.Result.RefreshToken
            };

            AuthorizationInfoRepository.Save(auth);
        }
    }
}
