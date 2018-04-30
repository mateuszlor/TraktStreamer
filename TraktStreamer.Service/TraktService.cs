using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using TraktApiSharp;
using TraktApiSharp.Authentication;
using TraktApiSharp.Enums;
using TraktApiSharp.Requests.Params;
using TraktStreamer.DAO.Model;
using TraktStreamer.Model;
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

        public async Task<ICollection<EpizodeModel>> GetAllEpizodesToWatchAsync(TraktClient client)
        {
            var watchlist = await client.Sync.GetWatchedShowsAsync();
            var toWatch = new List<EpizodeModel>();

            _logger.Debug("Got {0} series in watchlist", watchlist.Count());

            foreach (var show in watchlist)
            {
                var model = new SeriesModel
                {
                    TraktSlug = show.Show.Ids.Slug,
                    Name = show.Show.Title
                };

                var epizode = await GetEpizodeToWatch(client, model);

                if (epizode != null)
                {
                    toWatch.Add(epizode);
                }
            }

            // TODO sync with database
            // var series = toWatch.Select(e => e.Series.TraktSlug).Distinct();

            _logger.Debug("Got {0} epizodes to watch", toWatch.Count);

            return toWatch;
        }

        public async Task<EpizodeModel> GetEpizodeToWatch(TraktClient client, SeriesModel show)
        {
            var progress = await client.Shows.GetShowWatchedProgressAsync(show.TraktSlug);

            return progress.Aired > progress.Completed
                ? new EpizodeModel
                {
                    Series = show,
                    Season = progress.NextEpisode.SeasonNumber.GetValueOrDefault(),
                    Epizode = progress.NextEpisode.Number.GetValueOrDefault()
                }
                : null;
        }

        public async Task<TimeSpan> GetSpentTimeAsync(TraktClient client, TimeSpan? timeSpan = null)
        {
            var page = 1;

            var dateFrom = timeSpan.HasValue
                ? DateTime.Now.Date.Subtract(timeSpan.Value)
                : null as DateTime?;

            var watched = await client.Sync.GetWatchedHistoryAsync(limitPerPage: int.MaxValue, startAt:dateFrom, extendedInfo: new TraktExtendedInfo
            {
                Full = true
            });

            var minutes = watched.Sum(x => x.Episode?.Runtime ?? x.Movie?.Runtime ?? 0);

            var watchedTime = TimeSpan.FromMinutes(minutes);

            _logger.Debug("Watched {0} epizodes, {1} movies in {2} days {3} hours {4} minutes ({})",
                watched.Where(x => x.Type == TraktSyncItemType.Episode).Select(x => x.Episode.Ids.Trakt).Distinct().Count(),
                watched.Where(x => x.Type == TraktSyncItemType.Movie).Select(x => x.Movie.Ids.Trakt).Distinct().Count(),
                watchedTime.Days,
                watchedTime.Hours,
                watchedTime.Minutes,
                dateFrom.HasValue ? $"from {dateFrom.Value.Date}" : "EVER");

            return watchedTime;
        }
    }
}
