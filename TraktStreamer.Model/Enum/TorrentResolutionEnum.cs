using System;

namespace TraktStreamer.Model.Enum
{
    public enum TorrentResolutionEnum
    {
        All,
        _720p,
        _1080p,
        _4K
    }

    public static class TorrentResolutionEnumHelper
    {
        public static string ToPrettyString(this TorrentResolutionEnum value)
        {
            switch (value)
            {
                case TorrentResolutionEnum._4K:
                    return "4K | 2160p";
                case TorrentResolutionEnum._1080p:
                    return "1080p";
                case TorrentResolutionEnum._720p:
                    return "720p";
                default:
                    return string.Empty;
            }
        }
    }
}
