using UnityEngine;
using Newtonsoft.Json;

public class Json
{
    public static void SaveInPlayerPrefs(string key, object obj)
    {
        try
        {
            string jsonString = JsonConvert.SerializeObject(obj);
            PlayerPrefs.SetString(key, jsonString);
        }
        catch
        {
            Debug.LogError("[Json] ERROR: Couldn't save object in key " + key);
        }
        finally
        {
            Debug.Log("[Json] Saved object to key: " + key);
        }
    }

    public static object LoadFromPlayerPrefs(string key)
    {
        try
        {
            string jsonString = PlayerPrefs.GetString(key);
            return JsonConvert.DeserializeObject(jsonString);
        }
        catch
        {
            Debug.LogError("[Json] ERROR: Couldn't load object with key " + key);
            return null;
        }
        finally
        {
            Debug.Log("[Json] Loaded object from key: " + key);
        }
    }
}
