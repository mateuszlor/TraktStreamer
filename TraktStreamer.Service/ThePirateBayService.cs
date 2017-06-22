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
        public List<Torrent> Search(string name, TorrentResolutionEnum minimumResolution, bool bestResolutionOnly = true, bool applyDefaultLimits = true)
        {
            if (minimumResolution == TorrentResolutionEnum.All)
            {
                var torrentsAll = Tpb.Search(new Query(name, 0, QueryOrder.BySize));
                return torrentsAll.ToList();
            }

            var torrents = SearchSpecificResolution(name, TorrentResolutionEnum._1080p);

            if (applyDefaultLimits)
            {
                torrents = torrents.Where(t => t.Seeds > 0 && t.SizeBytes > 1 * 1024 * 1024 * 1024).ToList();
            }

            if (minimumResolution <= TorrentResolutionEnum._720p && (bestResolutionOnly || !torrents.Any()))
            {
                var torrents720 = SearchSpecificResolution(name, TorrentResolutionEnum._720p);

                torrents.AddRange(applyDefaultLimits
                    ? torrents720.Where(t => t.Seeds > 0 && t.SizeBytes > 500 * 1024 * 1024)
                    : torrents720);
            }

            return torrents.ToList();
        }

        private List<Torrent> SearchSpecificResolution(string name, TorrentResolutionEnum resolution)
        {
            var toSearch = $"{name} {resolution.ToPrettyString()}";
            return Tpb.Search(new Query(toSearch, 0, QueryOrder.BySize)).ToList();
        }
    }
}
