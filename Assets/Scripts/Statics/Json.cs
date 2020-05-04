using UnityEngine;
using Newtonsoft.Json;
using System;

public class Json
{
    public static void SaveInPlayerPrefs(string key, object obj)
    {
        try
        {
            string jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
            PlayerPrefs.SetString(key, jsonString);
            Debug.Log("[Json] Saved object to key: " + key);
        }
        catch (Exception e)
        {
            Debug.LogError("[Json] ERROR: Couldn't save object in key " + key + "\n" + e.Message);
        }
    }

    public static T LoadFromPlayerPrefs<T>(string key)
    {
        try
        {
            string jsonString = PlayerPrefs.GetString(key);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        catch (Exception e)
        {
            Debug.LogError("[Json] ERROR: Couldn't load object with key " + key + "\n" + e.Message);
            return default(T);
        }
    }
}
