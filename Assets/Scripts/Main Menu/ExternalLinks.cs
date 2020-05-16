using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ExternalLinks : MonoBehaviour
{
    public TMP_Text patreonTanks = null;

    public void OpenLink(string url)
    {
        if (url != "")
            Application.OpenURL(url);
        else
            Debug.LogWarning("[ExternalLinks] Trying to open empty URL!");
    }

    public void Start()
    {
        if (patreonTanks != null)
            StartCoroutine(GetDataAsync());
    }

    private IEnumerator GetDataAsync()
    {
        DownloadHandlerBuffer down = new DownloadHandlerBuffer();
        UnityWebRequest patreonRequest = new UnityWebRequest("https://www.patreon.com/api/campaigns/4321511", "GET", down, null);

        yield return patreonRequest.SendWebRequest();

        if (patreonRequest.isNetworkError)
        {
            Debug.Log("[ExternalLinks] Error: " + patreonRequest.error);
        }
        else
        {
            Debug.Log("[ExternalLinks] Received!");

            JObject rss = JObject.Parse(down.text);
            int patreonCount = (int)rss["data"]["attributes"]["patron_count"];
            patreonTanks.text = "Current Patrons\n<size= 0.13>" + patreonCount + "</size>\n\n" + patreonTanks.text;
        }
    }
}
