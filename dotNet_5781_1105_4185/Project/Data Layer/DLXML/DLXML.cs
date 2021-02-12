using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using DO;
using System.Globalization;

namespace DL
{
    sealed class DLXML : IDL
    {
        #region Singleton
        static readonly Lazy<DLXML> lazy = new Lazy<DLXML>(() => new DLXML());
        public static DLXML Instance => lazy.Value;

        private DLXML() { }
        #endregion

        private static string FileName<T>() => typeof(T).Name + ".xml";

        #region User
        public User GetUser(string name)
        {
            var users = XMLTools.LoadListFromXMLSerializer<User>(FileName<User>());

            return (from user in users
                    where user.Name == name
                    select user).FirstOrDefault();
        }
        public void AddUser(User user)
        {
            var users = XMLTools.LoadListFromXMLSerializer<User>(FileName<User>());
            if (users.Any(u => u.Name == user.Name))
                throw new BadUserNameException(user.Name, $"User with the name {user.Name} already exists");

            users.Add(user);

            XMLTools.SaveListToXMLSerializer(users, FileName<User>());
        }

        public void DeleteUser(User user)
        {
            var users = XMLTools.LoadListFromXMLSerializer<User>(FileName<User>());
            var exists = users.Find(u => u.Name == user.Name);
            if (exists == null) throw new BadUserNameException(user.Name, $"User with the name {user.Name} dosen't exists");

            users.Remove(exists);

            XMLTools.SaveListToXMLSerializer(users, FileName<User>());
        }
        #endregion

        #region Bus
        public IEnumerable<Bus> GetAllBuses()
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<Bus>());

            return from element in root.Elements()
                   select new Bus
                   {
                       RegNum = int.Parse(element.Element("RegNum").Value),
                       RegDate = DateTime.ParseExact(element.Element("RegDate").Value, "dd/MM/yyyy h:mm:ss", CultureInfo.InvariantCulture),
                       Kilometrage = int.Parse(element.Element("Kilometrage").Value),
                       FuelLeft = int.Parse(element.Element("FuelLeft").Value),
                       Status = (BusStatus)Enum.Parse(typeof(BusStatus), element.Element("Status").Value),
                       Type = (BusTypes)Enum.Parse(typeof(BusTypes), element.Element("Type").Value),
                   };
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            return from bus in GetAllBuses()
                   where predicate(bus)
                   select bus;
        }

        public Bus GetBus(int regNum)
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<Bus>());

            var result = (from element in root.Elements()
                          let busRegNum = int.Parse(element.Element("RegNum").Value)
                          where busRegNum == regNum
                          select new Bus
                          {
                              RegNum = regNum,
                              RegDate = DateTime.ParseExact(element.Element("RegDate").Value, "dd/MM/yyyy h:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                              Kilometrage = int.Parse(element.Element("Kilometrage").Value),
                              FuelLeft = int.Parse(element.Element("FuelLeft").Value),
                              Status = (BusStatus)Enum.Parse(typeof(BusStatus), element.Element("Status").Value),
                              Type = (BusTypes)Enum.Parse(typeof(BusTypes), element.Element("Type").Value),
                          }).FirstOrDefault();

            if (result == null)
            {
                throw new BadBusRegistrationException(regNum, $"Bus number {regNum} doesn't exists");
            }

            return result;
        }

        public void AddBus(Bus bus)
        {

            var root = XMLTools.LoadListFromXMLElement(FileName<Bus>());

            var busElem = (from element in root.Elements()
                           let busRegNum = int.Parse(element.Element("RegNum").Value)
                           where busRegNum == bus.RegNum
                           select element).FirstOrDefault();

            if (busElem != null)
            {
                throw new BadBusRegistrationException(
                    bus.RegNum, $"bus already exists");
            }

            root.Add(new XElement(nameof(Bus),
                new XElement("RegNum", bus.RegNum),
                new XElement("RegDate", bus.RegDate.ToString()),
                new XElement("Kilometrage", bus.Kilometrage),
                new XElement("FuelLeft", bus.FuelLeft),
                new XElement("Status", bus.Status),
                new XElement("Type", bus.Type)));

            XMLTools.SaveListToXMLElement(root, FileName<Bus>());
        }

        public void UpdateBus(Bus bus)
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<Bus>());

            var busElem = (from element in root.Elements()
                           let busRegNum = int.Parse(element.Element("RegNum").Value)
                           where busRegNum == bus.RegNum
                           select element).FirstOrDefault();

            if (busElem == null)
            {
                throw new BadBusRegistrationException(
                    bus.RegNum, $"bus doesn't exists");
            }

            busElem.Element("RegNum").Value = bus.RegNum.ToString();
            busElem.Element("RegDate").Value = bus.RegDate.ToString();
            busElem.Element("Kilometrage").Value = bus.Kilometrage.ToString();
            busElem.Element("FuelLeft").Value = bus.FuelLeft.ToString();
            busElem.Element("Status").Value = bus.Status.ToString();
            busElem.Element("Type").Value = bus.Type.ToString();

            XMLTools.SaveListToXMLElement(root, FileName<Bus>());
        }

        public void DeleteBus(int regNum)
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<Bus>());

            var busElem = (from element in root.Elements()
                           let busRegNum = int.Parse(element.Element("RegNum").Value)
                           where busRegNum == regNum
                           select element).FirstOrDefault();

            if (busElem == null)
            {
                throw new BadBusRegistrationException(regNum, $"bus doesn't exists");
            }

            busElem.Remove();

            XMLTools.SaveListToXMLElement(root, FileName<Bus>());
        }

        public void DeleteBusesBy(Predicate<Bus> predicate)
        {
            foreach (var bus in GetBusesBy(predicate))
            {
                DeleteBus(bus.RegNum);
            }
        }

        public void DeleteAllBuses()
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<Bus>());
            root.Elements().Remove();
            XMLTools.SaveListToXMLElement(root, FileName<Bus>());
        }

        #endregion

        #region Station
        public IEnumerable<Station> GetAllStations()
        {
            return XMLTools.LoadListFromXMLSerializer<Station>(FileName<Station>());
        }

        public IEnumerable<Station> GetStationsBy(Predicate<Station> predicate)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName<Station>());
            return from station in stations
                   where predicate(station)
                   select station;
        }

        public Station GetStation(int code)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName<Station>());

            var station = (from s in stations
                           where s.Code == code
                           select s).FirstOrDefault();
            if (station == null) throw new BadStationCodeException(code, $"Station with code {code} not found");

            return station;
        }

        public void AddStation(Station station)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName<Station>());
            if (stations.Any(s => s.Code == station.Code))
                throw new BadStationCodeException(station.Code, $"Station with code {station.Code} already exists");

            stations.Add(station);

            XMLTools.SaveListToXMLSerializer(stations, FileName<Station>());
        }

        public void UpdateStation(Station station)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName<Station>());
            var exists = stations.Find(s => s.Code == station.Code);
            if (exists == null) throw new BadStationCodeException(station.Code, $"no station with code {station.Code}");

            stations.Remove(exists);
            stations.Add(station);

            XMLTools.SaveListToXMLSerializer(stations, FileName<Station>());
        }

        public void DeleteStation(int code)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName<Station>());
            var exists = stations.Find(s => s.Code == code);
            if (exists == null) throw new BadStationCodeException(code, $"no station with code {code}");

            stations.Remove(exists);

            XMLTools.SaveListToXMLSerializer(stations, FileName<Station>());
        }

        public void DeleteStationsBy(Predicate<Station> predicate)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName<Station>());

            var updatedStations = from station in stations
                                  where !predicate(station)
                                  select station;

            XMLTools.SaveListToXMLSerializer(updatedStations.ToList(), FileName<Station>());
        }

        public void DeleteAllStations()
        {
            XMLTools.SaveListToXMLSerializer(new List<Station>(), FileName<Station>());
        }

        #endregion

        #region BusLine
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return XMLTools.LoadListFromXMLSerializer<BusLine>(FileName<BusLine>());
        }

        public IEnumerable<BusLine> GetBusLinesBy(Predicate<BusLine> predicate)
        {
            var busLines = XMLTools.LoadListFromXMLSerializer<BusLine>(FileName<BusLine>());
            return from busLine in busLines
                   where predicate(busLine)
                   select busLine;
        }

        public BusLine GetBusLine(Guid ID)
        {
            var busLines = XMLTools.LoadListFromXMLSerializer<BusLine>(FileName<BusLine>());

            var busLine = (from bl in busLines
                           where bl.ID == ID
                           select bl).FirstOrDefault();
            if (busLine == null) throw new BadBusLineIDException(ID, $"no bus line with ID {ID}");

            return busLine;
        }

        public void AddBusLine(BusLine busLine)
        {

            var busLines = XMLTools.LoadListFromXMLSerializer<BusLine>(FileName<BusLine>());
            if (busLines.Any(s => s.ID == busLine.ID))
                throw new BadBusLineIDException(busLine.ID, $"bus line with ID {busLine.ID} already exists");

            // Checks for the start and end stations
            if (busLine.StartStationCode != null) GetStation(busLine.StartStationCode ?? 0);
            if (busLine.EndStationCode != null) GetStation(busLine.EndStationCode ?? 0);

            if (busLine.HasFullRoute &&
                (busLine.StartStationCode == null ||
                busLine.EndStationCode == null))
                throw new InvalidOperationException("BusLine without start or stop stations cannot have full route");

            busLines.Add(busLine);

            XMLTools.SaveListToXMLSerializer(busLines, FileName<BusLine>());
        }

        public void UpdateBusLine(BusLine busLine)
        {
            var busLines = XMLTools.LoadListFromXMLSerializer<BusLine>(FileName<BusLine>());
            var exists = busLines.Find(bl => bl.ID == busLine.ID);
            if (exists == null) throw new BadBusLineIDException(busLine.ID, $"no bus line with ID {busLine.ID}");

            // Checks for the start and end stations
            if (busLine.StartStationCode != null) GetStation(busLine.StartStationCode ?? 0);
            if (busLine.EndStationCode != null) GetStation(busLine.EndStationCode ?? 0);

            if (busLine.HasFullRoute &&
                (busLine.StartStationCode == null ||
                busLine.EndStationCode == null))
                throw new InvalidOperationException("BusLine without start or stop stations cannot have full route");

            busLines.Remove(exists);
            busLines.Add(busLine);

            XMLTools.SaveListToXMLSerializer(busLines, FileName<BusLine>());
        }

        public void DeleteBusLine(Guid ID)
        {
            var busLines = XMLTools.LoadListFromXMLSerializer<BusLine>(FileName<BusLine>());
            var exists = busLines.Find(bl => bl.ID == ID);
            if (exists == null) throw new BadBusLineIDException(ID, $"no bus line with ID {ID}");

            busLines.Remove(exists);

            XMLTools.SaveListToXMLSerializer(busLines, FileName<BusLine>());
        }

        public void DeleteAllBusLines()
        {
            XMLTools.SaveListToXMLSerializer(new List<BusLine>(), FileName<BusLine>());
        }

        public void DeleteBusLinesBy(Predicate<BusLine> predicate)
        {
            var busLines = XMLTools.LoadListFromXMLSerializer<BusLine>(FileName<BusLine>());

            var updatedStations = from busLine in busLines
                                  where !predicate(busLine)
                                  select busLine;

            XMLTools.SaveListToXMLSerializer(updatedStations.ToList(), FileName<BusLine>());
        }
        #endregion

        #region LineStation

        public IEnumerable<LineStation> GetAllLineStations()
        {
            return XMLTools.LoadListFromXMLSerializer<LineStation>(FileName<LineStation>());
        }

        public IEnumerable<LineStation> GetLineStationsBy(Predicate<LineStation> predicate)
        {
            var lineStations = GetAllLineStations();
            return from ls in lineStations
                   where predicate(ls)
                   select ls;
        }

        public LineStation GetLineStationByIndex(Guid lineID, int index)
        {
            var lineStations = GetLineStationsBy(ls => ls.LineID == lineID);
            if (lineStations.Count() == 0)
                throw new BadBusLineIDException(lineID, $"no line station for line with id {lineID}");

            var lineStation = lineStations.FirstOrDefault(b => b.RouteIndex == index);

            if (lineStation == null) throw new BadLineStationIndexException(lineID, index,
                $"no line station with index {index} found for line ID {lineID}");

            return lineStation;
        }

        public LineStation GetLineStationByStation(Guid lineID, int stationCode)
        {
            var lineStations = GetLineStationsBy(ls => ls.LineID == lineID);
            if (lineStations.Count() == 0)
                throw new BadBusLineIDException(lineID, $"no line station for line with id {lineID}");

            var lineStation = lineStations.FirstOrDefault(b => b.StationCode == stationCode);

            if (lineStation == null) throw new BadLineStationStationCodeException(lineID, stationCode,
                $"no line station with station code {stationCode} found for line ID {lineID}");

            return lineStation;
        }

        public void AddLineStation(LineStation lineStation)
        {
            var lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(FileName<LineStation>());
            if (lineStations.Any(s => s.LineID == lineStation.LineID && s.StationCode == lineStation.StationCode))
                throw new BadLineStationStationCodeException(lineStation.LineID, lineStation.StationCode, $"Station {lineStation.StationCode} for line {lineStation.LineID} already exists");

            lineStations.Add(lineStation);

            XMLTools.SaveListToXMLSerializer(lineStations, FileName<LineStation>());
        }

        public void UpdateLineStationByIndex(LineStation lineStation)
        {
            var lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(FileName<LineStation>());
            var exists = lineStations.Find(s => s.LineID == lineStation.LineID && s.RouteIndex == lineStation.RouteIndex);
            if (exists == null) throw new BadLineStationStationCodeException(lineStation.LineID, lineStation.StationCode, $"no line station with code {lineStation.StationCode} for line {lineStation.LineID}");

            lineStations.Remove(exists);
            lineStations.Add(lineStation);

            XMLTools.SaveListToXMLSerializer(lineStations, FileName<LineStation>());
        }

        public void UpdateLineStationByStation(LineStation lineStation)
        {
            var lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(FileName<LineStation>());
            var exists = lineStations.Find(s => s.LineID == lineStation.LineID && s.StationCode == lineStation.StationCode);
            if (exists == null) throw new BadLineStationStationCodeException(lineStation.LineID, lineStation.StationCode, $"no line station with code {lineStation.StationCode} for line {lineStation.LineID}");

            lineStations.Remove(exists);
            lineStations.Add(lineStation);

            XMLTools.SaveListToXMLSerializer(lineStations, FileName<LineStation>());
        }

        public void DeleteLineStationByIndex(Guid lineID, int index)
        {
            var lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(FileName<LineStation>());
            var exists = lineStations.Find(s => s.LineID == lineID && s.RouteIndex == index);
            if (exists == null) throw new BadLineStationStationCodeException(lineID, exists.StationCode, $"no such line station for line {lineID}");

            lineStations.Remove(exists);

            XMLTools.SaveListToXMLSerializer(lineStations, FileName<LineStation>());
        }

        public void DeleteLineStationByStation(Guid lineID, int stationCode)
        {
            var lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(FileName<LineStation>());
            var exists = lineStations.Find(s => s.LineID == lineID && s.StationCode == stationCode);
            if (exists == null) throw new BadLineStationStationCodeException(lineID, stationCode, $"no line station with code {stationCode} for line {lineID}");

            lineStations.Remove(exists);

            XMLTools.SaveListToXMLSerializer(lineStations, FileName<LineStation>());
        }

        public void DeleteLineStationsBy(Predicate<LineStation> predicate)
        {
            var lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(FileName<LineStation>());

            var updatedLS = from ls in lineStations
                            where !predicate(ls)
                            select ls;

            XMLTools.SaveListToXMLSerializer(updatedLS.ToList(), FileName<LineStation>());
        }

        public void DeleteAllLineStations()
        {
            XMLTools.SaveListToXMLSerializer(new List<LineStation>(), FileName<LineStation>());
        }

        #endregion

        #region AdjacentStations
        public IEnumerable<AdjacentStations> GetAllAdjacentStations()
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<AdjacentStations>());

            return from element in root.Elements()
                   select new AdjacentStations
                   {
                       Station1Code = int.Parse(element.Element("Station1Code").Value),
                       Station2Code = int.Parse(element.Element("Station2Code").Value),
                       Distance = double.Parse(element.Element("Distance").Value),
                       DrivingTime = TimeSpan.ParseExact(
                            element.Element("DrivingTime").Value,
                            "hh\\:mm\\:ss",
                            CultureInfo.InvariantCulture)
                   };
        }

        public IEnumerable<AdjacentStations> GetAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            return from adj in GetAllAdjacentStations()
                   where predicate(adj)
                   select adj;
        }

        public AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2)
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<AdjacentStations>());

            var adj = (from element in root.Elements()
                       let code1 = int.Parse(element.Element("Station1Code").Value)
                       let code2 = int.Parse(element.Element("Station2Code").Value)
                       where stationCode1 == code1 && stationCode2 == code2 ||
                             stationCode1 == code2 && stationCode2 == code1
                       select new AdjacentStations
                       {
                           Station1Code = code1,
                           Station2Code = code2,
                           Distance = double.Parse(element.Element("Distance").Value),
                           DrivingTime = TimeSpan.ParseExact(
                                element.Element("DrivingTime").Value,
                                "hh\\:mm\\:ss",
                                CultureInfo.InvariantCulture)
                       }).FirstOrDefault();

            if (adj == null)
            {
                throw new BadAdjacentStationsCodeException(stationCode1, stationCode2,
                $"Stations with code {stationCode1} and {stationCode2} are not adjacents");
            }

            return adj;
        }

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            // Checks for stations.
            GetStation(adjacentStations.Station1Code);
            GetStation(adjacentStations.Station2Code);

            var root = XMLTools.LoadListFromXMLElement(FileName<AdjacentStations>());

            var adjElem = (from element in root.Elements()
                           let code1 = int.Parse(element.Element("Station1Code").Value)
                           let code2 = int.Parse(element.Element("Station2Code").Value)
                           where adjacentStations.Station1Code == code1 && adjacentStations.Station2Code == code2 ||
                                 adjacentStations.Station1Code == code2 && adjacentStations.Station2Code == code1
                           select element).FirstOrDefault();
            if (adjElem != null)
            {
                throw new BadAdjacentStationsCodeException(
                    adjacentStations.Station1Code, adjacentStations.Station2Code,
                    $"adjacent stations already exists");
            }

            root.Add(new XElement(nameof(AdjacentStations),
                new XElement("Station1Code", adjacentStations.Station1Code),
                new XElement("Station2Code", adjacentStations.Station2Code),
                new XElement("Distance", adjacentStations.Distance),
                new XElement("DrivingTime", adjacentStations.DrivingTime.ToString())
            ));

            XMLTools.SaveListToXMLElement(root, FileName<AdjacentStations>());
        }

        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<AdjacentStations>());

            var adjElem = (from element in root.Elements()
                           let code1 = int.Parse(element.Element("Station1Code").Value)
                           let code2 = int.Parse(element.Element("Station2Code").Value)
                           where adjacentStations.Station1Code == code1 && adjacentStations.Station2Code == code2 ||
                                 adjacentStations.Station1Code == code2 && adjacentStations.Station2Code == code1
                           select element).FirstOrDefault();
            if (adjElem == null)
            {
                throw new BadAdjacentStationsCodeException(
                    adjacentStations.Station1Code, adjacentStations.Station2Code,
                    $"no adjacent stations matches these stations");
            }

            adjElem.Element("Station1Code").Value = adjacentStations.Station1Code.ToString();
            adjElem.Element("Station2Code").Value = adjacentStations.Station2Code.ToString();
            adjElem.Element("Distance").Value = adjacentStations.Distance.ToString();
            adjElem.Element("DrivingTime").Value = adjacentStations.DrivingTime.ToString();

            XMLTools.SaveListToXMLElement(root, FileName<AdjacentStations>());
        }

        public void AddOrUpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            try
            {
                AddAdjacentStations(adjacentStations);
            }
            catch (BadAdjacentStationsCodeException)
            {
                UpdateAdjacentStations(adjacentStations);
            }
        }

        public void DeleteAdjacentStations(int stationCode1, int stationCode2)
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<AdjacentStations>());

            var adjElem = (from element in root.Elements()
                           let code1 = int.Parse(element.Element("Station1Code").Value)
                           let code2 = int.Parse(element.Element("Station2Code").Value)
                           where stationCode1 == code1 && stationCode2 == code2 ||
                                 stationCode1 == code2 && stationCode2 == code1
                           select element).FirstOrDefault();
            if (adjElem == null)
            {
                throw new BadAdjacentStationsCodeException(
                    stationCode1, stationCode2,
                    $"no adjacent stations matches these stations");
            }

            adjElem.Remove();

            XMLTools.SaveListToXMLElement(root, FileName<AdjacentStations>());
        }

        public void DeleteAllAdjacentStations()
        {
            var root = XMLTools.LoadListFromXMLElement(FileName<AdjacentStations>());
            root.Elements().Remove();
            XMLTools.SaveListToXMLElement(root, FileName<AdjacentStations>());
        }

        public void DeleteAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            foreach (var adj in GetAdjacentStationsBy(predicate))
            {
                DeleteAdjacentStations(adj.Station1Code, adj.Station2Code);
            }
        }
        #endregion
    }
}
