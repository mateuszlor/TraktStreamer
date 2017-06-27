using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePirateBay;
using TraktStreamer.Model.Enum;

namespace TraktStreamer.Service.API
{
    public interface IThePirateBayService
    {
        List<Torrent> Search(string name, TorrentResolutionEnum minimumResolution, bool bestResolutionOnly = true, bool applyDefaultLimits = true);

        Torrent SearchBest(string name);

        Torrent SearchBest(string name, double sizeTolerance);
    }
}
