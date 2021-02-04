using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DL
{
    static class XMLTools
    {
        const string DIRECTORY = @".\xml-data\";

        static XMLTools()
        {
            if (!Directory.Exists(DIRECTORY))
            {
                Directory.CreateDirectory(DIRECTORY);
            }
        }

        #region SaveLoadWithXElement
        public static void SaveListToXMLElement(XElement rootElem, string fileName)
        {
            try
            {
                rootElem.Save(DIRECTORY + fileName);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileException(fileName, $"fail to create xml file: {fileName}", ex);
            }
        }

        public static XElement LoadListFromXMLElement(string fileName)
        {
            try
            {
                if (File.Exists(DIRECTORY + fileName))
                {
                    return XElement.Load(DIRECTORY + fileName);
                }
                else
                {
                    XElement rootElem = new XElement(DIRECTORY + fileName);
                    rootElem.Save(DIRECTORY + fileName);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileException(fileName, $"fail to load xml file: {fileName}", ex);
            }
        }
        #endregion

        #region SaveLoadWithXMLSerializer
        public static void SaveListToXMLSerializer<T>(List<T> list, string fileName)
        {
            try
            {
                FileStream file = new FileStream(DIRECTORY + fileName, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileException(fileName, $"fail to create xml file: {fileName}", ex);
            }
        }
        public static List<T> LoadListFromXMLSerializer<T>(string fileName)
        {
            try
            {
                if (File.Exists(DIRECTORY + fileName))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(DIRECTORY + fileName, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileException(fileName, $"fail to load xml file: {fileName}", ex);
            }
        }
        #endregion
    }
}
