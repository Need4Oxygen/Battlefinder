using System;
using System.IO;
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

                Logger.Log("TYaml", $"Deserialized object of type \"{nameof(T)}\"");
                return (T)data;
            }
            catch (Exception e)
            {
                Logger.LogError("TYaml", $"Couldn't deserialize object of type \"{nameof(T)}\"\n{e.Message}\n{e.StackTrace}");
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

                Logger.Log("TYaml", $"Serialized object with name \"{obj.ToString()}\"");
                return data;
            }
            catch (Exception e)
            {
                Logger.LogError("TYaml", $"Couldn't serialize object \"{obj.ToString()}\"\n{e.Message}\n{e.StackTrace}");
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

                Logger.Log("TYaml", $"Deserialized object \"{Path.GetFileName(path)}\" of type \"{nameof(T)}\" from \"{path}\"");
                return data;
            }
            catch (Exception e)
            {
                Logger.LogError("TYaml", $"Couldn't deserialize object \"{Path.GetFileName(path)}\" of type \"{nameof(T)}\" from \"{path}\"\n{e.Message}\n{e.StackTrace}");
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

                Logger.Log("TYaml", $"Serialized object with name \"{fileName}\" into \"{path}\"");
            }
            catch (Exception e)
            {
                Logger.LogError("TYaml", $"Couldn't serialize object \"{fileName}\"\n{e.Message}\n{e.StackTrace}");
            }
        }

    }

}
