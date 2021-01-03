using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusLine
    {
        public Guid ID { get; set; }
        public int LineNum { get; set; }
        public Regions Region { get; set; }
        public int FirstStationCode { get; set; }
        public int LastStationCode { get; set; }
    }
}
