using System;
using System.Collections.Generic;
using System.Linq;
using ThePirateBay;
using TraktStreamer.Model.Enum;
using TraktStreamer.Service.API;

namespace TraktStreamer.Service
{
    public class ThePirateBayService : IThePirateBayService
    {
        public List<Torrent> Search(Torrent torrent, TorrentResolutionEnum minimumResolution, bool bestResolutionOnly = true, bool applyDefaultLimits = true)
        {
            return Search(torrent.ToString(), minimumResolution, bestResolutionOnly, applyDefaultLimits);
        }

        public List<Torrent> Search(string name, TorrentResolutionEnum minimumResolution, bool bestResolutionOnly = true, bool applyDefaultLimits = true)
        {
            if (minimumResolution == TorrentResolutionEnum.All)
            {
                var torrentsAll = Tpb.Search(new Query(name, 0, QueryOrder.BySize));
                return torrentsAll.ToList();
            }

            var torrents = SearchSpecificResolution(name, TorrentResolutionEnum._4K);

            if (applyDefaultLimits)
            {
                torrents = torrents.Where(t => t.Seeds > 0 && t.SizeBytes > 1 * 1024 * 1024 * 1024).ToList();
            }
            if (minimumResolution <= TorrentResolutionEnum._1080p && (bestResolutionOnly || !torrents.Any()))
            {
                var torrents1080 = SearchSpecificResolution(name, TorrentResolutionEnum._1080p);

                if (applyDefaultLimits)
                {
                    torrents1080 = torrents1080.Where(t => t.Seeds > 0 && t.SizeBytes > 1 * 1024 * 1024 * 1024).ToList();
                }

                torrents.AddRange(torrents1080);

                if (minimumResolution <= TorrentResolutionEnum._720p && (bestResolutionOnly || !torrents.Any()))
                {
                    var torrents720 = SearchSpecificResolution(name, TorrentResolutionEnum._720p);

                    torrents.AddRange(applyDefaultLimits
                        ? torrents720.Where(t => t.Seeds > 0 && t.SizeBytes > 500 * 1024 * 1024)
                        : torrents720);
                }
            }

            return torrents.ToList();
        }

        public Torrent SearchBest(Torrent torrent)
        {
            return SearchBest(torrent.ToString());
        }

        public Torrent SearchBest(string name)
        {
            return SearchBest(name, 0.9);
        }

        public Torrent SearchBest(Torrent torrent, double sizeTolerance)
        {
            return SearchBest(torrent.ToString(), sizeTolerance);
        }

        public Torrent SearchBest(string name, double sizeTolerance)
        {
            if (sizeTolerance < 0 || sizeTolerance > 1)
            {
                throw new ArgumentException("Size tolerance must be in <0,1>", nameof(sizeTolerance));
            }

            var torrents = Tpb.Search(new Query(name))
                .Where(t => t.Seeds > 0);

            var biggesSize = torrents
                .OrderByDescending(t => t.SizeBytes)
                .FirstOrDefault()
                ?.SizeBytes;

            if (biggesSize is null)
            {
                return null;
            }

            var sizeLimit = Convert.ToDecimal(sizeTolerance) * biggesSize.Value;

            return torrents
                .Where(t => t.SizeBytes > sizeLimit)
                .OrderByDescending(x => x.Seeds)
                .ThenByDescending(x => x.SizeBytes)
                .FirstOrDefault();
        }

        private List<Torrent> SearchSpecificResolution(string name, TorrentResolutionEnum resolution)
        {
            var toSearch = $"{name} {resolution.ToPrettyString()}";
            return Tpb.Search(new Query(toSearch, 0, QueryOrder.BySize)).ToList();
        }
    }
}
