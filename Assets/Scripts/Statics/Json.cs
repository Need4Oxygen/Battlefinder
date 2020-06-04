using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;

public class Json
{
    /// <summary>Serialize any object into a text file. Path should contain the separator but not the file name. </summary>
    /// <param name="fileName">File name should contain the file extension. </param>
    /// <param name="path">Path sould contain separator at the end. </param>
    public static void SerializeFile(object obj, string fileName, string path)
    {
        try
        {
            string jsonString = JsonConvert.SerializeObject(
                obj,
                Formatting.Indented,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            var sr = File.CreateText(path + fileName);
            sr.WriteLine(jsonString);
            sr.Close();

            // Debug.Log("[Json] Saved object with name: \"" + fileName + "\" into:\"" + path + "\"");
        }
        catch (Exception e)
        {
            Debug.LogError("[Json] ERROR: Couldn't save object with name: \"" + fileName + "\" into:\"" + path + "\"\n" + e.Message);
        }
    }

    /// <summary>Deserialize any object found in the given path into the specified type. </summary>
    public static T DeserializeFile<T>(string path)
    {
        try
        {
            string jsonString = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } });
        }
        catch (Exception e)
        {
            Debug.LogError("[Json] ERROR: Couldn't load object with key " + path + "\n" + e.Message + "\n" + e.StackTrace);
            return default(T);
        }
    }

    /// <summary>Serialize any object found in the given path into a string. </summary>
    /// <return>If seralization fails, empty string "" will be returned. </return>
    public static string Serialize(object obj)
    {
        try
        {
            string jsonString = JsonConvert.SerializeObject(
                obj,
                Formatting.None,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            // Debug.Log("[Json] Serialized object: " + obj.ToString());
            return jsonString;
        }
        catch (Exception e)
        {
            Debug.LogError("[Json] ERROR: Couldn't serialize object " + obj.ToString() + "\n" + e.Message);
            return "";
        }
    }

    /// <summary>Deserialize any object into the specified type. </summary>
    /// <return>If deseralization fails, default type will be returned. </return>
    public static T Deserialize<T>(string obj)
    {
        try
        {
            string jsonString = obj;
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        catch (Exception e)
        {
            Debug.LogError("[Json] ERROR: Couldn't deserialize object \n" + e.Message + "\n" + e.StackTrace);
            return default(T);
        }
    }
}
