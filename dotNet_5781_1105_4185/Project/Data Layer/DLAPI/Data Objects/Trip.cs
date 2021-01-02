using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI.DO
{
    public class Trip
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public Guid LineID { get; set; }
        public int StartStation { get; set; }
        public DateTime StartTime { get; set; }
        public int EndStation { get; set; }
        public DateTime EndTime { get; set; }
    }
}
