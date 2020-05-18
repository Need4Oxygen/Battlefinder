using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;

public class ExternalLinks : MonoBehaviour
{
    public TMP_Text patreonThanks = null;

    public void OpenLink(string url)
    {
        if (url != "")
            Application.OpenURL(url);
        else
            Debug.LogWarning("[ExternalLinks] Trying to open empty URL!");
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
            Debug.LogWarning("[ExternalLinks] Error: " + patreonRequest.error);
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
                Debug.LogError("[ExternalLinks] Error retrieving patreon count: \n" + e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
