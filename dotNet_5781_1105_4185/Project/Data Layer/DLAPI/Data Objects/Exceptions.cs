using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    #region User
    /// <summary>
    /// Bad user name exception
    /// </summary>
    public class BadUserNameException : Exception
    {
        // User's name
        public string Name { get; }

        public BadUserNameException(string name)
        {
            Name = name;
        }
        public BadUserNameException(string name, string message) : base(message)
        {
            Name = name;
        }
        public BadUserNameException(string name, string message, Exception innerException) : base(message, innerException)
        {
            Name = name;
        }
    }
    #endregion

    #region Station
    /// <summary>
    /// Bad station code exception
    /// </summary>
    public class BadStationCodeException : Exception
    {
        // Station's code
        public int Code { get; }

        public BadStationCodeException(int code)
        {
            Code = code;
        }
        public BadStationCodeException(int code, string message) : base(message)
        {
            Code = code;
        }
        public BadStationCodeException(int code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
    #endregion

    #region Bus
    /// <summary>
    /// <br>Bad bus registration exception</br>
    /// <br>For example: 7 digits after 2018</br>
    /// </summary>
    public class BadBusRegistrationException : Exception
    {
        // Bus's registration number
        public int RegNum { get; }
        // Bus's registration date
        public DateTime? RegDate { get; }
        public BadBusRegistrationException(int regNum, DateTime? regDate = null)
        {
            RegNum = regNum;
            RegDate = regDate;
        }
        public BadBusRegistrationException(int regNum, string message) : base(message)
        {
            RegNum = RegNum;
        }
        public BadBusRegistrationException(int regNum, DateTime regDate, string message) : base(message)
        {
            RegNum = RegNum;
            RegDate = regDate;
        }
        public BadBusRegistrationException(int regNum, string message, Exception innerException) : base(message, innerException)
        {
            RegNum = RegNum;
        }
        public BadBusRegistrationException(int regNum, DateTime regDate, string message, Exception innerException) : base(message, innerException)
        {
            RegNum = RegNum;
            RegDate = regDate;
        }
    }
    #endregion

    #region BusLine
    /// <summary>
    /// Bad bus line exception
    /// </summary>
    public class BadBusLineIDException : Exception
    {
        // BusLine's ID
        public Guid ID { get; }

        public BadBusLineIDException(Guid id)
        {
            ID = id;
        }
        public BadBusLineIDException(Guid id, string message) : base(message)
        {
            ID = id;
        }
        public BadBusLineIDException(Guid id, string message, Exception innerException) : base(message, innerException)
        {
            ID = id;
        }
    }
    #endregion

    #region LineStation
    /// <summary>
    /// Bad LineStation code exception
    /// </summary>
    public class BadLineStationStationCodeException : Exception
    {
        // LineStation's ID
        public Guid LineID { get; }
        // Station's code
        public int StationCode { get; }

        public BadLineStationStationCodeException(Guid id, int stationCode)
        {
            LineID = id;
            StationCode = stationCode;
        }
        public BadLineStationStationCodeException(Guid id, int stationCode, string message) : base(message)
        {
            LineID = id;
            StationCode = stationCode;
        }
        public BadLineStationStationCodeException(Guid id, int stationCode, string message, Exception innerException) : base(message, innerException)
        {
            LineID = id;
            StationCode = stationCode;
        }
    }

    /// <summary>
    /// Bad LineStation index exception
    /// </summary>
    public class BadLineStationIndexException : Exception
    {
        // LineStation's ID
        public Guid LineID { get; }
        // Station's index
        public int Index { get; }

        public BadLineStationIndexException(Guid id, int index)
        {
            LineID = id;
            Index = index;
        }
        public BadLineStationIndexException(Guid id, int index, string message) : base(message)
        {
            LineID = id;
            Index = index;
        }
        public BadLineStationIndexException(Guid id, int index, string message, Exception innerException) : base(message, innerException)
        {
            LineID = id;
            Index = index;
        }
    }
    #endregion

    #region AdjacentStations
    /// <summary>
    /// Bad adjacent stations code exception
    /// </summary>
    public class BadAdjacentStationsCodeException : Exception
    {
        // First station code
        public int Station1Code { get; }
        // Second station code
        public int Station2Code { get; }

        public BadAdjacentStationsCodeException(int code1, int code2)
        {
            Station1Code = code1;
            Station2Code = code2;
        }
        public BadAdjacentStationsCodeException(int code1, int code2, string message) : base(message)
        {
            Station1Code = code1;
            Station2Code = code2;
        }
        public BadAdjacentStationsCodeException(int code1, int code2, string message, Exception innerException) : base(message, innerException)
        {
            Station1Code = code1;
            Station2Code = code2;
        }
    }
    #endregion

    #region LineTrip
    public class BadLineTripIDAndTimeException : Exception
    {
        public Guid LineID { get; }
        public TimeSpan StartTime { get; }

        public BadLineTripIDAndTimeException(Guid lineID, TimeSpan startTime)
        {
            LineID = lineID;
            StartTime = startTime;
        }
        public BadLineTripIDAndTimeException(Guid lineID, TimeSpan startTime, string message) : base(message)
        {
            LineID = lineID;
            StartTime = startTime;
        }
        public BadLineTripIDAndTimeException(Guid lineID, TimeSpan startTime, string message, Exception innerException) : base(message, innerException)
        {
            LineID = lineID;
            StartTime = startTime;
        }
    }

    public class BadLineTripFrequencyAndFinishTime : Exception
    {
        public LineTrip LineTrip { get; }

        public BadLineTripFrequencyAndFinishTime(LineTrip lineTrip)
        {
            LineTrip = lineTrip;
        }
        public BadLineTripFrequencyAndFinishTime(LineTrip lineTrip, string message) : base(message)
        {
            LineTrip = lineTrip;
        }
        public BadLineTripFrequencyAndFinishTime(LineTrip lineTrip, string message, Exception innerException) : base(message, innerException)
        {
            LineTrip = lineTrip;
        }
    }
    #endregion

    #region XML
    /// <summary>
    /// Bad XML exception
    /// </summary>
    public class XMLFileException : Exception
    {
        // The xml file's name
        public string FileName { get; }

        public XMLFileException(string fileName)
        {
            FileName = fileName;
        }
        public XMLFileException(string fileName, string message) : base(message)
        {
            FileName = fileName;
        }
        public XMLFileException(string fileName, string message, Exception innerException) : base(message, innerException)
        {
            FileName = fileName;
        }
    }
    #endregion
}
