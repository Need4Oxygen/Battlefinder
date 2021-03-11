using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class ExternalLinks : MonoBehaviour
{
    public TMP_Text patreonThanks = null;

    public void OpenLink(string url)
    {
        if (url != "")
            Application.OpenURL(url);
        else
            Logger.LogWarning("ExternalLinks", $"Trying to open empty URL!");
    }

    public void Start()
    {
        if (patreonThanks != null)
            StartCoroutine(GetDataAsync());
    }

    private IEnumerator GetDataAsync()
    {
        DownloadHandlerBuffer down = new DownloadHandlerBuffer();
        UnityWebRequest patreonRequest = new UnityWebRequest("https://www.patreon.com/api/campaigns/4321511", "GET", down, null);

        yield return patreonRequest.SendWebRequest();

        if (patreonRequest.isNetworkError)
        {
            Logger.LogWarning("ExternalLinks", $"Couldn't retrieve patreon count\n{patreonRequest.error}");
        }
        else
        {
            try
            {
                JObject rss = JObject.Parse(down.text);
                int patreonCount = (int)rss["data"]["attributes"]["patron_count"];
                patreonThanks.text = "Current Patrons\n<size= 0.13>" + patreonCount + "</size>\n\n" + patreonThanks.text;
            }
            catch (Exception e)
            {
                Logger.LogError("ExternalLinks", $"Couldn't retrieve patreon count\n{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
