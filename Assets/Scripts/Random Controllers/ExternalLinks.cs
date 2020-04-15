using UnityEngine;

public class ExternalLinks : MonoBehaviour
{
    public void OpenLink(string url)
    {
        if (url != "")
            Application.OpenURL(url);
        else
            Debug.LogWarning("[ExternalLinks] Trying to open empty URL!");
    }
}
