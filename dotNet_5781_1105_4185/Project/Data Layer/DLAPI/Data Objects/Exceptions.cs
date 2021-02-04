using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    #region User
    public class BadUserNameException : Exception
    {
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
    public class BadStationCodeException : Exception
    {
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
    public class BadBusRegistrationException : Exception
    {
        public int RegNum { get; }
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
    public class BadBusLineIDException : Exception
    {
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
    public class BadLineStationStationCodeException : Exception
    {
        public Guid LineID { get; }
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
    public class BadLineStationIndexException : Exception
    {
        public Guid LineID { get; }
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
    public class BadAdjacentStationsCodeException : Exception
    {
        public int Station1Code { get; }
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

    #region XML
    public class XMLFileException : Exception
    {
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
