﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineStation
    {
        public Guid LineID { get; set; }
        public int StationCode { get; set; }
        public int RouteIndex { get; set; }
    }
}