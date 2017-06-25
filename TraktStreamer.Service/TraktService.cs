using System;
using System.Threading.Tasks;
using NLog;
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

        private Logger _logger = LogManager.GetCurrentClassLogger();

        public async Task<TraktClient> GetAuthorizedTraktClientAsync(Func<string, string> userInputCallback)
        {
            var client = new TraktClient(CLIENT_ID, CLIENT_SECRET);

            var auth = AuthorizationInfoRepository.GetLast();

            if (auth is null)
            {
                var authorizationUrl = client.OAuth.CreateAuthorizationUrl();
                
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

            _logger.Info("Authorized successfully");
            _logger.Debug("AccessToken: {0}, RefreshToken: {1}", client.Authorization.AccessToken, client.Authorization.RefreshToken);

            return client;
        }
    }
}
