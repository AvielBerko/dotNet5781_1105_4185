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
        public static List<DO.Trip> Trips;
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
                    Region = DO.Regions.Center,
                },
                new DO.BusLine
                {
                    ID = Guid.NewGuid(),
                    LineNum = 39,
                    Region = DO.Regions.Jerusalem,
                },
            };

            DrivingBuses = new List<DO.DrivingBus>
            {

            };

            Stations = new List<DO.Station>
            {
                 new DO.Station
                {
                    Code = 38831,
                    Name = "בי''ס בר לב/בן יהודה",
                    Address = "בן יהודה 76 כפר סבא",
                    Latitude = 32.183921,
                    Longitude = 34.917806,
                },
                new DO.Station
                {
                    Code = 38832,
                    Name = "הרצל/צומת בילו",
                    Address = "הרצל קרית עקרון",
                    Latitude = 31.870034,
                    Longitude = 34.819541,
                },
                new DO.Station
                {
                    Code = 38833,
                    Name = "הנחשול/הדייגים",
                    Address = "הנחשול 30 ראשון לציון",
                    Latitude = 31.984553,
                    Longitude = 34.782828,
                },
                new DO.Station
                {
                    Code = 38834,
                    Name = "פריד/ששת הימים",
                    Address = "משה פריד 9 רחובות",
                    Latitude = 31.88855,
                    Longitude = 34.790904,
                },
                new DO.Station
                {
                    Code = 38836,
                    Name = "ת. מרכזית לוד/הורדה",
                    Address = "לוד",
                    Latitude = 31.956392,
                    Longitude = 34.898098,
                },
                new DO.Station
                {
                    Code = 38837,
                    Name = "חנה אברך/וולקני",
                    Address = "חנה אברך 9 רחובות",
                    Latitude = 31.892166,
                    Longitude = 34.796071,
                },
                new DO.Station
                {
                    Code = 38838,
                    Name = "הרצל/משה שרת",
                    Address = "הרצל 20 קרית עקרון",
                    Latitude = 31.857565,
                    Longitude = 34.824106,
                },
                new DO.Station
                {
                    Code = 38839,
                    Name = "הבנים/אלי כהן",
                    Address = "הבנים 4 קרית עקרון",
                    Latitude = 31.862305,
                    Longitude = 34.821857,
                },
                new DO.Station
                {
                    Code = 38840,
                    Name = "ויצמן/הבנים",
                    Address = "וייצמן 11 קרית עקרון",
                    Latitude = 31.865085,
                    Longitude = 34.822237,
                },
                new DO.Station
                {
                    Code = 38841,
                    Name = "האירוס/הכלנית",
                    Address = "האירוס 13 קרית עקרון",
                    Latitude = 31.865222,
                    Longitude = 34.818957,
                },
                new DO.Station
                {
                    Code = 39285,
                    Name = "וייצמן/שרת",
                    Address = "שדרות חיים וייצמן 86 נתניה",
                    Latitude = 32.339039,
                    Longitude = 34.859959,
                },
                new DO.Station
                {
                    Code = 39286,
                    Name = "וייצמן/איכילוב",
                    Address = "שדרות חיים וייצמן 95 נתניה",
                    Latitude = 32.339836,
                    Longitude = 34.859845,
                },
                new DO.Station
                {
                    Code = 39287,
                    Name = "הרב הרצוג/שדרות חיים וייצמן",
                    Address = "שדרות חיים וייצמן 109 נתניה",
                    Latitude = 32.341324,
                    Longitude = 34.86009,
                },
                new DO.Station
                {
                    Code = 39288,
                    Name = "שד.וייצמן/סוקולוב",
                    Address = "שדרות חיים וייצמן 110 נתניה",
                    Latitude = 32.341515,
                    Longitude = 34.86033,
                },
                new DO.Station
                {
                    Code = 39289,
                    Name = "בית הבאר/נחום סוקולוב",
                    Address = "נחום סוקולוב 19 נתניה",
                    Latitude = 32.342087,
                    Longitude = 34.860981,
                },
                new DO.Station
                {
                    Code = 39291,
                    Name = "הרב ריינס/עמק חפר",
                    Address = "הרב ריינס 41 נתניה",
                    Latitude = 32.336476,
                    Longitude = 34.865621,
                },
                new DO.Station
                {
                    Code = 39292,
                    Name = "גולומב/טרומפלדור",
                    Address = "אליהו גולומב 45 נתניה",
                    Latitude = 32.341185,
                    Longitude = 34.863344,
                },
                new DO.Station
                {
                    Code = 39293,
                    Name = "שרת/אליהו גולומב",
                    Address = "אליהו גולומב 31 נתניה",
                    Latitude = 32.339896,
                    Longitude = 34.86404,
                },
                new DO.Station
                {
                    Code = 39294,
                    Name = "נחום סוקולוב/פייר ז'ילבר",
                    Address = "נחום סוקולוב 41 נתניה",
                    Latitude = 32.341611,
                    Longitude = 34.865289,
                },
                new DO.Station
                {
                    Code = 39295,
                    Name = "נחום סוקולוב/פייר ז'ילבר",
                    Address = "נחום סוקולוב 64 נתניה",
                    Latitude = 32.34141,
                    Longitude = 34.86566,
                },
                new DO.Station
                {
                    Code = 39296,
                    Name = "בית חולים לניאדו",
                    Address = "הרב מיימון 4 נתניה",
                    Latitude = 32.34622,
                    Longitude = 34.857721,
                },
                new DO.Station
                {
                    Code = 39297,
                    Name = "הגדוד העברי/דברי חיים",
                    Address = "דברי חיים 47 נתניה",
                    Latitude = 32.34798,
                    Longitude = 34.857421,
                },
                new DO.Station
                {
                    Code = 39298,
                    Name = "אח''י דקר/דברי חיים",
                    Address = "דברי חיים 34 נתניה",
                    Latitude = 32.34778,
                    Longitude = 34.857511,
                },
                new DO.Station
                {
                    Code = 39299,
                    Name = "הרב חי טייב/הרב מיימון",
                    Address = "הרב מיימון 5 נתניה",
                    Latitude = 32.34633,
                    Longitude = 34.857925,
                },
                new DO.Station
                {
                    Code = 39300,
                    Name = "הרב מיימון/מוריה",
                    Address = "הרב מיימון 30 נתניה",
                    Latitude = 32.345629,
                    Longitude = 34.860975,
                },
                new DO.Station
                {
                    Code = 41772,
                    Name = "ניצנים/פינסקי",
                    Address = "ניצנים 16 חיפה",
                    Latitude = 32.804984,
                    Longitude = 34.977793,
                },
                new DO.Station
                {
                    Code = 41773,
                    Name = "פינסקי/כלניות",
                    Address = "דוד פינסקי 39 חיפה",
                    Latitude = 32.80333,
                    Longitude = 34.979124,
                },
                new DO.Station
                {
                    Code = 41774,
                    Name = "פינסקי/הורדים",
                    Address = "דוד פינסקי 23 חיפה",
                    Latitude = 32.802685,
                    Longitude = 34.980566,
                },
                new DO.Station
                {
                    Code = 41778,
                    Name = "טובים/סלמן",
                    Address = "טובים חיפה",
                    Latitude = 32.809209,
                    Longitude = 35.020237,
                },
                new DO.Station
                {
                    Code = 41779,
                    Name = "מסעף אושה",
                    Address = "7703 זבולון",
                    Latitude = 32.794116,
                    Longitude = 35.112309,
                },
                new DO.Station
                {
                    Code = 41780,
                    Name = "מסעף כפר המכבי",
                    Address = "7703 זבולון",
                    Latitude = 32.792545,
                    Longitude = 35.117681,
                },
                new DO.Station
                {
                    Code = 41781,
                    Name = "העצמאות/מורדי הגטאות",
                    Address = "העצמאות 43 קרית אתא",
                    Latitude = 32.804168,
                    Longitude = 35.103924,
                },
                new DO.Station
                {
                    Code = 41782,
                    Name = "העצמאות/הילל",
                    Address = "העצמאות 29 קרית אתא",
                    Latitude = 32.802423,
                    Longitude = 35.103737,
                },
                new DO.Station
                {
                    Code = 41784,
                    Name = "מסעף כפר גלים",
                    Address = "4 טירת כרמל",
                    Latitude = 32.765262,
                    Longitude = 34.96209,
                },
                new DO.Station
                {
                    Code = 41785,
                    Name = "שפרינצק",
                    Address = "שדרות ההגנה חיפה",
                    Latitude = 32.822204,
                    Longitude = 34.956488,
                },
                new DO.Station
                {
                    Code = 41786,
                    Name = "מוריה/פינסקי",
                    Address = "שדרות מוריה 10 חיפה",
                    Latitude = 32.801906,
                    Longitude = 34.983813,
                },
                new DO.Station
                {
                    Code = 41787,
                    Name = "הגיבורים/שמידט",
                    Address = "הגיבורים 90 חדרה",
                    Latitude = 32.439389,
                    Longitude = 34.923829,
                },
                new DO.Station
                {
                    Code = 41788,
                    Name = "אחד העם/שד. רוטשילד",
                    Address = "אחד העם 20 חדרה",
                    Latitude = 32.434276,
                    Longitude = 34.920598,
                },
                new DO.Station
                {
                    Code = 41790,
                    Name = "דרך עכו/שדרות חן",
                    Address = "דרך עכו חיפה קרית ביאליק",
                    Latitude = 32.861697,
                    Longitude = 35.095013,
                },
                new DO.Station
                {
                    Code = 41791,
                    Name = "דרך עכו/שדרות חן",
                    Address = "דרך עכו חיפה קרית מוצקין",
                    Latitude = 32.859993,
                    Longitude = 35.094512,
                },
                new DO.Station
                {
                    Code = 41792,
                    Name = "מסעף נחל נעמן",
                    Address = "דרך עכו חיפה קרית ביאליק",
                    Latitude = 32.873256,
                    Longitude = 35.096147,
                },
                new DO.Station
                {
                    Code = 41794,
                    Name = "בי''ס אזורי",
                    Address = "דרך הבנים 37 פרדס חנה כרכור",
                    Latitude = 32.476904,
                    Longitude = 34.981335,
                },
                new DO.Station
                {
                    Code = 41796,
                    Name = "טשרניחובסקי/ישעיהו",
                    Address = "טשרניחובסקי 22 חיפה",
                    Latitude = 32.819901,
                    Longitude = 34.975053,
                },
                new DO.Station
                {
                    Code = 41797,
                    Name = "העצמאות/הנביאים",
                    Address = "העצמאות 22 קרית אתא",
                    Latitude = 32.801427,
                    Longitude = 35.103659,
                },
                new DO.Station
                {
                    Code = 41799,
                    Name = "הדקלים/תמר",
                    Address = "הדקלים 36 פרדס חנה כרכור",
                    Latitude = 32.467719,
                    Longitude = 34.977669,
                },
            };

            LineStations = new List<DO.LineStation>
            {

            };

            AdjacentStations = new List<DO.AdjacentStations>
            {
            };

            Trips = new List<DO.Trip>
            {

            };

            LineTrips = new List<DO.LineTrip>
            {

            };
        }
    }
}
