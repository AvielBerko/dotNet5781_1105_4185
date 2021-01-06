using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum BusStatus { Ready, NeedRefuel, NeedTreatment, Driving, Refueling, InTreatment }
    public enum BusTypes { Single, Double, DoubleDecker }
    public enum Roles { Normal, Admin }
}
