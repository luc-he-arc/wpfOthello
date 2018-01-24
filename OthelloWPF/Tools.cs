using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace OthelloWPF
{
    //https://stackoverflow.com/questions/6115721/how-to-save-restore-serializable-object-to-from-file
    static class Tools
    {        
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="serializableObject">the object you want to serialize</param>
        /// <param name="fileName">the path where you want to save it</param>
        public static void SerializeObjectBinary<T>(T serialazable, string fileName)
        {
            if (serialazable == null) { return; }

            FileStream stream = File.OpenWrite(fileName);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, serialazable);
            stream.Close();
        }

        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize</typeparam>
        /// <param name="fileName">the path where of the file you want to deserialize</param>
        /// <returns>the object deserialized</returns>
        public static T DeSerializeObjectBinary<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            FileStream stream = File.OpenRead(fileName);
            var formatter = new BinaryFormatter();
            T v = (T) formatter.Deserialize(stream);
            stream.Close();

            return v;
        }
    }
}
