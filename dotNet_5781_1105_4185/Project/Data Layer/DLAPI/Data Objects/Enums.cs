using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI.DO
{
    public enum BusStatus { Ready, NeedRefuel, NeedTreatment, Driving, Refueling, InTreatment }
    public enum Regions { TelAviv, Jerusalem, Haifa, South, Center, North, WestBank, GolanHeights }
    public enum Roles { Normal, Admin }
}
