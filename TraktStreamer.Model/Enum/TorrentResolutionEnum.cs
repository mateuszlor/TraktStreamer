using System;

namespace TraktStreamer.Model.Enum
{
    public enum TorrentResolutionEnum
    {
        All,
        _720p,
        _1080p
    }

    public static class TorrentResolutionEnumHelper
    {
        public static string ToString(this TorrentResolutionEnum value)
        {
            switch (value)
            {
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
