using System;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TYaml
{

    public sealed class Serialization
    {

        /// <summary> Deserialize any object into the specified type. </summary>
        /// <return> If deseralization fails, default type will be returned. </return>
        public static T Deserialize<T>(string value)
        {
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new UnderscoredNamingConvention())
                    .IgnoreUnmatchedProperties()
                    .Build();

                var reader = new StringReader(value);
                var data = deserializer.Deserialize<T>(reader);
                reader.Close();

                return (T)data;
            }
            catch (Exception e)
            {
                Debug.LogError("[Yaml] ERROR: Couldn't deserialize object \n" + e.Message + "\n" + e.StackTrace);
                return default(T);
            }
        }

        /// <summary> Serialize any object found in the given path into a string. </summary>
        /// <return> If seralization fails, empty string "" will be returned. </return>
        public static string Serialize(object obj)
        {
            try
            {
                string data = Serialize(obj);

                return data;
            }
            catch (Exception e)
            {
                Debug.LogError("[Yaml] ERROR: Couldn't serialize object " + obj.ToString() + "\n" + e.Message);
                return "";
            }
        }

        /// <summary> Deserialize any object found in the given path into the specified type. </summary>
        public static T DeserializeFile<T>(string path)
        {
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new UnderscoredNamingConvention())
                    .IgnoreUnmatchedProperties()
                    .Build();


                var data = deserializer.Deserialize<T>(File.ReadAllText(path));

                return data;
            }
            catch (Exception e)
            {
                Debug.LogError("[Yaml] ERROR: Couldn't load object with key " + path + "\n" + e.Message + "\n" + e.StackTrace);
                return default(T);
            }
        }

        /// <summary> Serialize any object found in the given path into a string. </summary>
        /// <return> If seralization fails, empty string "" will be returned. </return>
        public static void SerializeFile(object obj, string fileName, string path)
        {
            try
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(new UnderscoredNamingConvention())
                    .Build();

                TextWriter tw = File.CreateText(path + fileName);
                serializer.Serialize(tw, obj);

                tw.Close();

                Debug.Log("[Yaml] Saved object with name: \"" + fileName + "\" into:\"" + path + "\"");
            }
            catch (Exception e)
            {
                Debug.LogError("[Yaml] ERROR: Couldn't serialize object " + obj.ToString() + "\n" + e.Message);
            }
        }

    }

}
