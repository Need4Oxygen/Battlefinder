using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExternalLinks : MonoBehaviour
{
    public void OpenLink(string url)
    {
        if (url != "")
            Application.OpenURL(url);
        else
            Debug.LogWarning("[ExternalLinks] Trying to open empty URL!");
    }


    // public string accessToken;

    // public void Start()
    // {
    //     StartCoroutine(GetDataAsync());
    // }

    // private IEnumerator GetDataAsync()
    // {
    //     Dictionary<string, string> headers = new Dictionary<string, string>();
    //     headers.Add("authorization", "Bearer " + accessToken);
    //     DownloadHandlerBuffer down = new DownloadHandlerBuffer();
    //     UnityWebRequest patreonRequest = new UnityWebRequest("https://www.patreon.com/api/oauth2/api/current_user", "HTTP GET", down, null);

    //     yield return patreonRequest.SendWebRequest();

    //     Debug.Log(patreonRequest.responseCode);
    //     Debug.Log(down.text);
    // }

}
