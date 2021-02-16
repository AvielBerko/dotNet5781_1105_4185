using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    static class DataSet
    {
        public static List<DO.User> Users;
        public static List<DO.Bus> Buses;
        public static List<DO.BusLine> Lines;
        public static List<DO.DrivingBus> DrivingBuses;
        public static List<DO.Station> Stations;
        public static List<DO.LineStation> LineStations;
        public static List<DO.AdjacentStations> AdjacentStations;
        public static List<DO.LineTrip> LineTrips;

        static DataSet()
        {
            InitData();
        }

        static void InitData()
        {
            Users = new List<DO.User>
            {
                new DO.User
                {
                    Name = "admin",
                    Password = "admin",
                    Role = DO.Roles.Admin
                },
                new DO.User
                {
                    Name = "user",
                    Password = "user",
                    Role = DO.Roles.Normal
                }
            };

            Buses = new List<DO.Bus>
            {
                 new DO.Bus
                {
                     RegNum = 12345678,
                     RegDate = DateTime.Parse("23/01/2020"),
                     Kilometrage = 2119,
                     FuelLeft = 1000,
                     Status = DO.BusStatus.Ready,
                     Type = DO.BusTypes.Single
                },
                new DO.Bus
                {
                     RegNum = 1018593,
                     RegDate = DateTime.Parse("23/01/2001"),
                     Kilometrage = 100233 ,
                     FuelLeft = 142,
                     Status = DO.BusStatus.NeedRefuel,
                     Type = DO.BusTypes.Single
                },
                 new DO.Bus
                {
                     RegNum = 69203434,
                     RegDate = DateTime.Parse("17/04/2019"),
                     Kilometrage = 29403,
                     FuelLeft = 698,
                     Status = DO.BusStatus.Ready,
                     Type = DO.BusTypes.Double
                }
            };

            Lines = new List<DO.BusLine>
            {
                new DO.BusLine
                {
                    ID = Guid.NewGuid(),
                    LineNum = 75,
                    Region = DO.Regions.Jerusalem,
                    StartStationCode = 174,
                    EndStationCode = 175,
                    HasFullRoute = true,
                    RouteLength = 2,
                },
                new DO.BusLine
                {
                    ID = Guid.NewGuid(),
                    LineNum = 39,
                    Region = DO.Regions.Jerusalem,
                    StartStationCode = 175,
                    EndStationCode = 176,
                    HasFullRoute = true,
                    RouteLength = 2,
                },
            };

            DrivingBuses = new List<DO.DrivingBus>
            {

            };

            Stations = new List<DO.Station>
            {
                    new DO.Station
                {
                    Code = 174,
                    Name = "גבעת שאול/קוטלר",
                    Address = "גבעת שאול 9 ירושלים",
                    Latitude = 31.791653,
                    Longitude = 35.194683,
                },
                new DO.Station
                {
                    Code = 175,
                    Name = "גבעת שאול/קוטלר",
                    Address = "גבעת שאול 20 ירושלים",
                    Latitude = 31.791889,
                    Longitude = 35.194251,
                },
                new DO.Station
                {
                    Code = 176,
                    Name = "גבעת שאול/נג'ארה",
                    Address = "גבעת שאול 17 ירושלים",
                    Latitude = 31.792105,
                    Longitude = 35.192721,
                },
                new DO.Station
                {
                    Code = 177,
                    Name = "גבעת שאול/נג'ארה",
                    Address = "רבי ישראל נג'ארה 2 ירושלים",
                    Latitude = 31.792076,
                    Longitude = 35.191927,
                },
                new DO.Station
                {
                    Code = 179,
                    Name = "נג'ארה/בן עוזיאל",
                    Address = "רבי ישראל נג'ארה 30 ירושלים",
                    Latitude = 31.789376,
                    Longitude = 35.191168,
                },
                new DO.Station
                {
                    Code = 180,
                    Name = "כפר שאול/קצנלבוגן",
                    Address = "הרב רפאל קצנלבוגן 96 ירושלים",
                    Latitude = 31.786054,
                    Longitude = 35.179234,
                },
                new DO.Station
                {
                    Code = 181,
                    Name = "קצנלבוגן/הפלאה",
                    Address = "הרב רפאל קצנלבוגן 79 ירושלים",
                    Latitude = 31.784193,
                    Longitude = 35.177902,
                },
                new DO.Station
                {
                    Code = 182,
                    Name = "ישיבת ויזניץ/קצנלבוגן",
                    Address = "הרב רפאל קצנלבוגן 57 ירושלים",
                    Latitude = 31.782798,
                    Longitude = 35.177047,
                },
                new DO.Station
                {
                    Code = 184,
                    Name = "קצנלבוגן/הקבלן",
                    Address = "הרב רפאל קצנלבוגן 19 ירושלים",
                    Latitude = 31.787168,
                    Longitude = 35.175009,
                },
                new DO.Station
                {
                    Code = 186,
                    Name = "קצנלבוגן/מעלות בוסטון",
                    Address = "הרב רפאל קצנלבוגן 5 ירושלים",
                    Latitude = 31.788321,
                    Longitude = 35.173541,
                },
                new DO.Station
                {
                    Code = 187,
                    Name = "שאולזון/משקלוב",
                    Address = "הרב ש. שאולזון 23 ירושלים",
                    Latitude = 31.789146,
                    Longitude = 35.172418,
                },
                new DO.Station
                {
                    Code = 188,
                    Name = "שאולזון/מעלות בוסטון",
                    Address = "הרב ש. שאולזון 46 ירושלים",
                    Latitude = 31.787328,
                    Longitude = 35.172764,
                },
                new DO.Station
                {
                    Code = 189,
                    Name = "שאולזון/רוז'ין",
                    Address = "הרב ש. שאולזון 55 ירושלים",
                    Latitude = 31.785602,
                    Longitude = 35.17396,
                },
                new DO.Station
                {
                    Code = 190,
                    Name = "שאולזון/זרח ברנט",
                    Address = "הרב ש. שאולזון 78 ירושלים",
                    Latitude = 31.78312,
                    Longitude = 35.173535,
                },
                new DO.Station
                {
                    Code = 191,
                    Name = "חי טייב/שאולזון",
                    Address = "הרב יצחק חי טייב 55 ירושלים",
                    Latitude = 31.781717,
                    Longitude = 35.173116,
                },
                new DO.Station
                {
                    Code = 192,
                    Name = "חי טייב/אבן דנן",
                    Address = "הרב יצחק חי טייב 43 ירושלים",
                    Latitude = 31.783243,
                    Longitude = 35.171828,
                },
                new DO.Station
                {
                    Code = 193,
                    Name = "חי טייב/ברנד",
                    Address = "הרב יצחק חי טייב ירושלים",
                    Latitude = 31.785651,
                    Longitude = 35.172418,
                },
                new DO.Station
                {
                    Code = 194,
                    Name = "חי טייב/רוזנטל",
                    Address = "הרב יצחק חי טייב 3 ירושלים",
                    Latitude = 31.78833,
                    Longitude = 35.170991,
                },
                new DO.Station
                {
                    Code = 195,
                    Name = "רוזנטל/שאולזון",
                    Address = "הרב אברהם דוד רוזנטל ירושלים",
                    Latitude = 31.790435,
                    Longitude = 35.177365,
                },
                new DO.Station
                {
                    Code = 197,
                    Name = "הפסגה/שד' הרצל",
                    Address = "הפסגה 3 ירושלים",
                    Latitude = 31.770974,
                    Longitude = 35.182288,
                },
                new DO.Station
                {
                    Code = 199,
                    Name = "הפסגה/הרשד''ם",
                    Address = "הפסגה 13 ירושלים",
                    Latitude = 31.769614,
                    Longitude = 35.182558,
                },
                new DO.Station
                {
                    Code = 200,
                    Name = "הרוזמרין/כביש המנהרות",
                    Address = "הרוזמרין 57 ירושלים",
                    Latitude = 31.732231,
                    Longitude = 35.202069,
                },
                new DO.Station
                {
                    Code = 201,
                    Name = "דב יוסף/יערי",
                    Address = "דב יוסף ירושלים",
                    Latitude = 31.734036,
                    Longitude = 35.194675,
                },
                new DO.Station
                {
                    Code = 202,
                    Name = "אצטדיון טדי/א''ס ביתר",
                    Address = "דרך אגודת ספורט בית''ר ירושלים",
                    Latitude = 31.751193,
                    Longitude = 35.18933,
                },
                new DO.Station
                {
                    Code = 203,
                    Name = "חניון הלאום",
                    Address = "שדרות הנשיא השישי 2 ירושלים",
                    Latitude = 31.783061,
                    Longitude = 35.203237,
                },
                new DO.Station
                {
                    Code = 207,
                    Name = "3 שכונת בזבז",
                    Address = "דרך בית לחם הישנה ירושלים",
                    Latitude = 31.770693,
                    Longitude = 35.243402,
                },
                new DO.Station
                {
                    Code = 210,
                    Name = "שמואל הנביא/תדהר",
                    Address = "שמואל הנביא 71 ירושלים",
                    Latitude = 31.793299,
                    Longitude = 35.222176,
                },
                new DO.Station
                {
                    Code = 211,
                    Name = "שמואל הנביא/יקים",
                    Address = "שמואל הנביא 47 ירושלים",
                    Latitude = 31.791394,
                    Longitude = 35.22416,
                },
                new DO.Station
                {
                    Code = 213,
                    Name = "זקס",
                    Address = "משה זקס 5 ירושלים",
                    Latitude = 31.789828,
                    Longitude = 35.225822,
                },
                new DO.Station
                {
                    Code = 214,
                    Name = "שמואל הנביא/יקים",
                    Address = "שמואל הנביא 48 ירושלים",
                    Latitude = 31.791505,
                    Longitude = 35.224204,
                },
                new DO.Station
                {
                    Code = 215,
                    Name = "שמואל הנביא/תדהר",
                    Address = "שמואל הנביא 70 ירושלים",
                    Latitude = 31.792975,
                    Longitude = 35.222797,
                },
                new DO.Station
                {
                    Code = 218,
                    Name = "ירמיהו/אלקנה",
                    Address = "ירמיהו ירושלים",
                    Latitude = 31.792925,
                    Longitude = 35.213412,
                },
                new DO.Station
                {
                    Code = 221,
                    Name = "משרד החוץ/שד' רבין",
                    Address = "שדרות יצחק רבין ירושלים",
                    Latitude = 31.782889,
                    Longitude = 35.202207,
                },
                new DO.Station
                {
                    Code = 222,
                    Name = "אצטדיון טדי/א''ס ביתר",
                    Address = "דרך אגודת ספורט בית''ר ירושלים",
                    Latitude = 31.751345,
                    Longitude = 35.188474,
                },
                new DO.Station
                {
                    Code = 223,
                    Name = "רוזמרין/לבונה",
                    Address = "הרוזמרין 8 ירושלים",
                    Latitude = 31.732731,
                    Longitude = 35.195877,
                },
                new DO.Station
                {
                    Code = 224,
                    Name = "כביש 60/הרוזמרין",
                    Address = "יציאה לכביש 60 ירושלים",
                    Latitude = 31.731185,
                    Longitude = 35.202031,
                },
                new DO.Station
                {
                    Code = 227,
                    Name = "אנג'ל/כנפי נשרים",
                    Address = "כנפי נשרים 6 ירושלים",
                    Latitude = 31.787712,
                    Longitude = 35.192306,
                },
                new DO.Station
                {
                    Code = 228,
                    Name = "מרכז שטנר/כנפי נשרים",
                    Address = "כנפי נשרים 20 ירושלים",
                    Latitude = 31.787703,
                    Longitude = 35.188761,
                },
                new DO.Station
                {
                    Code = 229,
                    Name = "בית ענבר/כנפי נשרים",
                    Address = "כנפי נשרים ירושלים",
                    Latitude = 31.787728,
                    Longitude = 35.185498,
                },
                new DO.Station
                {
                    Code = 230,
                    Name = "רוזנטל/כנפי נשרים",
                    Address = "הרב אברהם דוד רוזנטל ירושלים",
                    Latitude = 31.787754,
                    Longitude = 35.179644,
                },
                new DO.Station
                {
                    Code = 232,
                    Name = "קצנלבוגן/מעלות בוסטון",
                    Address = "הרב רפאל קצנלבוגן 16 ירושלים",
                    Latitude = 31.788077,
                    Longitude = 35.173651,
                },
                new DO.Station
                {
                    Code = 234,
                    Name = "דבר ירושלים/קצנלבוגן",
                    Address = "הרב רפאל קצנלבוגן 58 ירושלים",
                    Latitude = 31.783625,
                    Longitude = 35.174993,
                },
                new DO.Station
                {
                    Code = 235,
                    Name = "ישיבת ויזניץ/קצנלבוגן",
                    Address = "הרב רפאל קצנלבוגן 74 ירושלים",
                    Latitude = 31.782684,
                    Longitude = 35.177453,
                },
                new DO.Station
                {
                    Code = 236,
                    Name = "קצנלבוגן/הפלאה",
                    Address = "הרב רפאל קצנלבוגן 88 ירושלים",
                    Latitude = 31.784417,
                    Longitude = 35.177826,
                },
                new DO.Station
                {
                    Code = 237,
                    Name = "כפר שאול/קצנלבוגן",
                    Address = "הרב רפאל קצנלבוגן 94 ירושלים",
                    Latitude = 31.785614,
                    Longitude = 35.178884,
                },
                new DO.Station
                {
                    Code = 239,
                    Name = "מכללת ולקאן/דרך יריחו",
                    Address = "דרך יריחו ירושלים",
                    Latitude = 31.771694,
                    Longitude = 35.25023,
                },
                new DO.Station
                {
                    Code = 242,
                    Name = "הפסגה/תורה ועבודה",
                    Address = "הפסגה 36 ירושלים",
                    Latitude = 31.766926,
                    Longitude = 35.183562,
                },
                new DO.Station
                {
                    Code = 244,
                    Name = "דרך יריחו/אל ופא",
                    Address = "דרך יריחו ירושלים",
                    Latitude = 31.771715,
                    Longitude = 35.250334,
                },
                new DO.Station
                {
                    Code = 246,
                    Name = "צומת עמר בן אלעס",
                    Address = "עומר אבן אל עאס 7 ירושלים",
                    Latitude = 31.786182,
                    Longitude = 35.230219,
                },
                new DO.Station
                {
                    Code = 254,
                    Name = "משרד המשפטים/צלאח א דין",
                    Address = "סלאח א דין 31 ירושלים",
                    Latitude = 31.78718,
                    Longitude = 35.23007,
                },
                new DO.Station
                {
                    Code = 255,
                    Name = "משרד המשפטים/צלאח א דין",
                    Address = "סלאח א דין 36 ירושלים",
                    Latitude = 31.787402,
                    Longitude = 35.229997,
                },
                new DO.Station
                {
                    Code = 256,
                    Name = "דרך שכם/לואי ונסן",
                    Address = "דרך שכם 60 ירושלים",
                    Latitude = 31.789166,
                    Longitude = 35.228845,
                },
                new DO.Station
                {
                    Code = 257,
                    Name = "דרך שכם/לואי ונסן",
                    Address = "דרך שכם 55 ירושלים",
                    Latitude = 31.789365,
                    Longitude = 35.229001,
                },
                new DO.Station
                {
                    Code = 263,
                    Name = "דרך שכם/טובלר",
                    Address = "דרך שכם 45 ירושלים",
                    Latitude = 31.792394,
                    Longitude = 35.22901,
                },
                new DO.Station
                {
                    Code = 264,
                    Name = "דרך שכם/טובלר",
                    Address = "דרך שכם 45 ירושלים",
                    Latitude = 31.792304,
                    Longitude = 35.229086,
                },
                new DO.Station
                {
                    Code = 265,
                    Name = "בית ספר א סכאכיני",
                    Address = "דרך שכם 17 ירושלים",
                    Latitude = 31.793122,
                    Longitude = 35.230291,
                },
                new DO.Station
                {
                    Code = 273,
                    Name = "שאולזון/רוזנטל",
                    Address = "הרב ש. שאולזון 3 ירושלים",
                    Latitude = 31.790632,
                    Longitude = 35.17594,
                },
                new DO.Station
                {
                    Code = 276,
                    Name = "בית ענבר/כנפי נשרים",
                    Address = "כנפי נשרים 21 ירושלים",
                    Latitude = 31.787528,
                    Longitude = 35.184601,
                },
                new DO.Station
                {
                    Code = 277,
                    Name = "מרכז שטנר/כנפי נשרים",
                    Address = "כנפי נשרים 11 ירושלים",
                    Latitude = 31.78751,
                    Longitude = 35.188178,
                },
                new DO.Station
                {
                    Code = 278,
                    Name = "אנג'ל/כנפי נשרים",
                    Address = "כנפי נשרים ירושלים",
                    Latitude = 31.787504,
                    Longitude = 35.192121,
                },
                new DO.Station
                {
                    Code = 279,
                    Name = "פרבשטיין/בית הדפוס",
                    Address = "יהושוע פרבשטיין 7 ירושלים",
                    Latitude = 31.78599,
                    Longitude = 35.192338,
                },
                new DO.Station
                {
                    Code = 280,
                    Name = "האדמור מלובאויטש/דרוק",
                    Address = "האדמו''ר מליובאוויטש 8 ירושלים",
                    Latitude = 31.809878,
                    Longitude = 35.21414,
                },
                new DO.Station
                {
                    Code = 282,
                    Name = "חזון איש/האדמור מלובאויטש",
                    Address = "חזון אי''ש 44 ירושלים",
                    Latitude = 31.808332,
                    Longitude = 35.220311,
                },
                new DO.Station
                {
                    Code = 284,
                    Name = "קהילות יעקב",
                    Address = "קהילות יעקב ירושלים",
                    Latitude = 31.812803,
                    Longitude = 35.218441,
                },
                new DO.Station
                {
                    Code = 290,
                    Name = "ז'ולטי/חזון איש",
                    Address = "הרב בצלאל זולטי 28 ירושלים",
                    Latitude = 31.809776,
                    Longitude = 35.217392,
                },
                new DO.Station
                {
                    Code = 291,
                    Name = "מאיר חדש/חזון איש",
                    Address = "הרב מאיר חדש 11 ירושלים",
                    Latitude = 31.810077,
                    Longitude = 35.218886,
                },
                new DO.Station
                {
                    Code = 292,
                    Name = "מאיר חדש/אגרות משה",
                    Address = "הרב מאיר חדש ירושלים",
                    Latitude = 31.810092,
                    Longitude = 35.220306,
                },
                new DO.Station
                {
                    Code = 293,
                    Name = "אגרות משה/קלכהיים",
                    Address = "איגרות משה 25 ירושלים",
                    Latitude = 31.809603,
                    Longitude = 35.221798,
                },
                new DO.Station
                {
                    Code = 294,
                    Name = "אגרות משה/חזון אי''ש",
                    Address = "איגרות משה 1 ירושלים",
                    Latitude = 31.808295,
                    Longitude = 35.225043,
                },
                new DO.Station
                {
                    Code = 295,
                    Name = "אגרות משה/חזון איש",
                    Address = "איגרות משה 2 ירושלים",
                    Latitude = 31.80829,
                    Longitude = 35.225149,
                },
                new DO.Station
                {
                    Code = 300,
                    Name = "ז'ולטי/חזון איש",
                    Address = "הרב בצלאל זולטי 26 ירושלים",
                    Latitude = 31.809917,
                    Longitude = 35.217285,
                },
                new DO.Station
                {
                    Code = 301,
                    Name = "ז'ולטי",
                    Address = "הרב בצלאל זולטי 14 ירושלים",
                    Latitude = 31.810505,
                    Longitude = 35.215835,
                },
                new DO.Station
                {
                    Code = 302,
                    Name = "ז'ולטי/דרוק",
                    Address = " ירושלים",
                    Latitude = 31.812307,
                    Longitude = 35.214896,
                },
                new DO.Station
                {
                    Code = 304,
                    Name = "דרוק",
                    Address = "רבי שלמה זלמן דרוק 66 ירושלים",
                    Latitude = 31.814367,
                    Longitude = 35.217481,
                },
                new DO.Station
                {
                    Code = 305,
                    Name = "דרוק/קהילות יעקב",
                    Address = "רבי שלמה זלמן דרוק 82 ירושלים",
                    Latitude = 31.813442,
                    Longitude = 35.218937,
                },
                new DO.Station
                {
                    Code = 306,
                    Name = "קהילות יעקב",
                    Address = "קהילות יעקב ירושלים",
                    Latitude = 31.812894,
                    Longitude = 35.218443,
                },
                new DO.Station
                {
                    Code = 307,
                    Name = "ברכת אברהם",
                    Address = "ברכת אברהם 24 ירושלים",
                    Latitude = 31.811524,
                    Longitude = 35.217366,
                },
                new DO.Station
                {
                    Code = 312,
                    Name = "האדמור מלובאויטש/דרוק",
                    Address = "האדמו''ר מליובאוויטש 1 ירושלים",
                    Latitude = 31.810543,
                    Longitude = 35.213754,
                },
                new DO.Station
                {
                    Code = 313,
                    Name = "בית חולים סן ג'וזף",
                    Address = "מוסא סייק 2 ירושלים",
                    Latitude = 31.795766,
                    Longitude = 35.230404,
                },
                new DO.Station
                {
                    Code = 314,
                    Name = "בית חולים סן ג'וזף",
                    Address = "דרך שכם ירושלים",
                    Latitude = 31.795736,
                    Longitude = 35.230796,
                },
                new DO.Station
                {
                    Code = 315,
                    Name = "הקונגרס הציוני/רקאנטי",
                    Address = "הקונגרס הציוני ירושלים",
                    Latitude = 31.818045,
                    Longitude = 35.193628,
                },
                new DO.Station
                {
                    Code = 316,
                    Name = "רקאנטי/בן זאב",
                    Address = "אברהם רקנאטי 10 ירושלים",
                    Latitude = 31.817176,
                    Longitude = 35.192797,
                },
                new DO.Station
                {
                    Code = 317,
                    Name = "רקאנטי/אידלזון",
                    Address = "אברהם רקנאטי 44 ירושלים",
                    Latitude = 31.818019,
                    Longitude = 35.189665,
                },
                new DO.Station
                {
                    Code = 318,
                    Name = "רקאנטי/המשורר אצ''ג",
                    Address = "אברהם רקנאטי ירושלים",
                    Latitude = 31.820307,
                    Longitude = 35.187729,
                },
                new DO.Station
                {
                    Code = 319,
                    Name = "קלרמון גנו/אהרון קציר",
                    Address = "קלרמון גאנו 3 ירושלים",
                    Latitude = 31.795857,
                    Longitude = 35.233157,
                },
                new DO.Station
                {
                    Code = 329,
                    Name = "רמות/סולם יעקב",
                    Address = "שדרות רמות ירושלים",
                    Latitude = 31.821138,
                    Longitude = 35.200326,
                },
                new DO.Station
                {
                    Code = 330,
                    Name = "רמות/מעוז",
                    Address = "שדרות רמות ירושלים",
                    Latitude = 31.81859,
                    Longitude = 35.202461,
                },
                new DO.Station
                {
                    Code = 331,
                    Name = "שיבת ציון/לואי ליפסקי",
                    Address = "שיבת ציון ירושלים",
                    Latitude = 31.816761,
                    Longitude = 35.201404,
                },
                new DO.Station
                {
                    Code = 332,
                    Name = "שיבת ציון/טללים",
                    Address = "שיבת ציון ירושלים",
                    Latitude = 31.815446,
                    Longitude = 35.199314,
                },
                new DO.Station
                {
                    Code = 333,
                    Name = "שיבת ציון/טללים",
                    Address = "שיבת ציון ירושלים",
                    Latitude = 31.815281,
                    Longitude = 35.199427,
                },
                new DO.Station
                {
                    Code = 334,
                    Name = "שיבת ציון/שירת הים",
                    Address = "שיבת ציון ירושלים",
                    Latitude = 31.816595,
                    Longitude = 35.201537,
                },
                new DO.Station
                {
                    Code = 335,
                    Name = "רמות/מעוז",
                    Address = "שדרות רמות ירושלים",
                    Latitude = 31.818485,
                    Longitude = 35.202563,
                },
                new DO.Station
                {
                    Code = 336,
                    Name = "רמות/סולם יעקב",
                    Address = "שדרות רמות ירושלים",
                    Latitude = 31.82141,
                    Longitude = 35.199843,
                },
                new DO.Station
                {
                    Code = 345,
                    Name = "הקונגרס הציוני/גולדה",
                    Address = "הקונגרס הציוני ירושלים",
                    Latitude = 31.817292,
                    Longitude = 35.194052,
                },
                new DO.Station
                {
                    Code = 346,
                    Name = "תחנת דלק/לוחמי הגטאות",
                    Address = "לוחמי הגטאות ירושלים",
                    Latitude = 31.800369,
                    Longitude = 35.241982,
                },
                new DO.Station
                {
                    Code = 350,
                    Name = "המ''ג/זכרון יעקב",
                    Address = "המ''ג 34 ירושלים",
                    Latitude = 31.794802,
                    Longitude = 35.199621,
                },
                new DO.Station
                {
                    Code = 352,
                    Name = "פנים מאירות",
                    Address = "פנים מאירות 14 ירושלים",
                    Latitude = 31.795494,
                    Longitude = 35.2018,
                },
                new DO.Station
                {
                    Code = 363,
                    Name = "מעגלי הרי''ם לוין/מבוא פתיה",
                    Address = "מעגלי הרי''ם לוין 6 ירושלים",
                    Latitude = 31.801378,
                    Longitude = 35.216731,
                },
                new DO.Station
                {
                    Code = 365,
                    Name = "מעגלי הרי''ם לוין/מעלות דושינסקי",
                    Address = "מעגלי הרי''ם לוין 14 ירושלים",
                    Latitude = 31.803276,
                    Longitude = 35.217104,
                },
                new DO.Station
                {
                    Code = 366,
                    Name = "מעגלי הרי''ם לוין/מבוא השלום והאחדות",
                    Address = "מעגלי הרי''ם לוין 28 ירושלים",
                    Latitude = 31.802616,
                    Longitude = 35.219359,
                },
            };

            LineStations = new List<DO.LineStation>
            {
                new DO.LineStation
                {
                    LineID = Lines[0].ID,
                    StationCode = 174,
                    RouteIndex = 0,
                },
                new DO.LineStation
                {
                    LineID = Lines[0].ID,
                    StationCode = 175,
                    RouteIndex = 1,
                },
                new DO.LineStation
                {
                    LineID = Lines[1].ID,
                    StationCode = 175,
                    RouteIndex = 0,
                },
                new DO.LineStation
                {
                    LineID = Lines[1].ID,
                    StationCode = 176,
                    RouteIndex = 1,
                },
            };

            AdjacentStations = new List<DO.AdjacentStations>
            {
                new DO.AdjacentStations
                {
                    Station1Code = 174,
                    Station2Code = 175,
                    Distance = 303.34,
                    DrivingTime = TimeSpan.FromMinutes(4.20),
                },
                new DO.AdjacentStations
                {
                    Station1Code = 175,
                    Station2Code = 176,
                    Distance = 295.46,
                    DrivingTime = TimeSpan.FromMinutes(1.90),
                },
            };

            LineTrips = new List<DO.LineTrip>
            {
                new DO.LineTrip
                {
                    LineID = Lines[0].ID,
                    StartTime = new TimeSpan(11, 5, 0),
                    FinishTime = new TimeSpan(22, 30, 0),
                    Frequency = new TimeSpan(0, 30, 0),
                },
                new DO.LineTrip
                {
                    LineID = Lines[0].ID,
                    StartTime = new TimeSpan(9, 0, 0),
                },
            };
        }
    }
}
