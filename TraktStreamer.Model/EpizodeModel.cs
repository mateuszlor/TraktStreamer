using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktStreamer.Model
{
    public class EpizodeModel:BaseApplicationModel
    {
        public SeriesModel Series { get; set; }
        public int Season { get; set; }
        public int Epizode { get; set; }

        public override string ToString()
        {
            return $"{Series.Name} S{Season:00}E{Epizode:00}";
        }
    }
}
