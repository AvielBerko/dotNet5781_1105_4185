using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class AdjacentStations
    {
        public Station Station1 { get; set; }
        public Station Station2 { get; set; }
        public double Distance { get; set; }
        public TimeSpan DrivingTime { get; set; }

        public Station GetOtherStation(int fromStationCode)
        {
            if (fromStationCode == Station1.Code)
            {
                return Station2;
            }

            if (fromStationCode == Station2.Code)
            {
                return Station1;
            }

            throw new BadStationCodeException(fromStationCode, $"Station with code {fromStationCode} not foud");
        }
    }
}
