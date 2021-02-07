using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Tools.Utils
{
    /// <summary>
    /// Serialization manager
    /// </summary>
    /// <typeparam name="T">Type of object to serialize/deserialize</typeparam>
    public static class SerializerUtils<T> where T : class
    {
        /// <summary>
        /// Serialize an object into a file with XML format.
        /// </summary>
        /// <param name="o">Object to serialize.</param>
        /// <param name="file">File where the object need to be store.</param>
        public static void XmlSerialize(T o, string file)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            FileStream writer = new FileStream(file, FileMode.Create);
            xmlSerializer.Serialize(writer, o);
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Deserialize an object from XML file.
        /// </summary>
        /// <param name="file">Path of the file to deserialize.</param>
        /// <returns>Object fill with file informations.</returns>
        public static T XmlDeserialize(string file)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            FileStream reader = new FileStream(file, FileMode.Open);
            T result = xmlSerializer.Deserialize(reader) as T;
            reader.Close();
            reader.Dispose();

            return result;
        }

        /// <summary>
        /// Serialize an object into a file with JSON format.
        /// </summary>
        /// <param name="o">Object to serialize.</param>
        /// <param name="file">File where the object need to be store.</param>
        public static void JsonSerialize(T o, string file)
        {
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T), settings);

            FileStream writer = new FileStream(file, FileMode.Create);
            ser.WriteObject(writer, o);
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Deserialize an object from JSON file.
        /// </summary>
        /// <param name="file">Path of the file to deserialize.</param>
        /// <returns>Object fill with file informations.</returns>
        public static T JsonDeserialize(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(T));

            // Deserialize the data and read it from the instance.
            T result = ser.ReadObject(reader, true) as T;
            reader.Close();
            reader.Dispose();
            fs.Close();
            fs.Dispose();

            return result;
        }

        /// <summary>
        /// Serialize an object into a file with XML format and keep references.
        /// </summary>
        /// <param name="o">Object to serialize.</param>
        /// <param name="file">File where the object need to be store.</param>
        public static void DataContractSerialize(T o, string file)
        {
            DataContractSerializerSettings settings = new DataContractSerializerSettings();
            settings.MaxItemsInObjectGraph = int.MaxValue;
            settings.PreserveObjectReferences = true;
            DataContractSerializer ser = new DataContractSerializer(typeof(T), settings);

            FileStream writer = new FileStream(file, FileMode.Create);
            ser.WriteObject(writer, o);
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Deserialize an object from XML file and keep references.
        /// </summary>
        /// <param name="file">Path of the file to deserialize.</param>
        /// <returns>Object fill with file informations.</returns>
        public static T DataContractDeserialize(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(T));

            // Deserialize the data and read it from the instance.
            T result = ser.ReadObject(reader, true) as T;
            reader.Close();
            reader.Dispose();
            fs.Close();
            fs.Dispose();

            return result;
        }
    }
}
