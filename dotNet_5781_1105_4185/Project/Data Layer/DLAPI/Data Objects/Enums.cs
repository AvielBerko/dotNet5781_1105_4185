using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Bus Statuses
    /// </summary>
    public enum BusStatus { Ready, NeedRefuel, NeedTreatment, Driving, Refueling, InTreatment }
    /// <summary>
    /// Bus Types
    /// </summary>
    public enum BusTypes { Single, Double, DoubleDecker }
    /// <summary>
    /// Bus Regions of operation
    /// </summary>
    public enum Regions { TelAviv, Jerusalem, Haifa, South, Center, North, WestBank, GolanHeights }
    /// <summary>
    /// Users roles
    /// </summary>
    public enum Roles { Normal, Admin }

}
