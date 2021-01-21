using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum NotifSprites
{
    _default, _deco1, _deco2, _deco3, _deco4, _deco5, _exclamation,
    _tick, _cross, _arrow
};
public enum NotifColors { _default, _green, _orange, _red, _blue, _black };

public class NotificationManager : MonoBehaviour
{
    //singleton
    public static NotificationManager Instance { get; private set; }

    [Serializable]
    public struct PicAndName
    {
        public NotifSprites spriteName;
        public Sprite sprite;
    }

    [Serializable]
    public struct ColorAndName
    {
        public NotifColors notifColor;
        public Color color;
    }

    [SerializeField] Transform dontDestroy = null;
    [SerializeField] GameObject messageStandarPrefab = default;
    [SerializeField] RectTransform canvas = default;

    public List<PicAndName> spriteList = new List<PicAndName>();
    public List<ColorAndName> colorList = new List<ColorAndName>();
    public List<GameObject> activeNotifications = new List<GameObject>();
    Dictionary<NotifSprites, Sprite> spriteDic = new Dictionary<NotifSprites, Sprite>();
    Dictionary<NotifColors, Color> colorDic = new Dictionary<NotifColors, Color>();

    //singleton stuff
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(dontDestroy);
            Instance = this;
        }
        else
            Destroy(dontDestroy.gameObject);
    }

    void Start()
    {
        foreach (PicAndName item in spriteList)
            spriteDic.Add(item.spriteName, item.sprite);
        foreach (ColorAndName item in colorList)
            colorDic.Add(item.notifColor, item.color);
    }

    public void Notify(NotifColors colorName, NotifSprites spriteName, string text, float lifespan, float delay)
    {
        if (delay > 0)
            StartCoroutine(NotifyWithDelay(colorName, spriteName, text, lifespan, delay));
        else
            Notify(colorName, spriteName, text, lifespan);
    }
    IEnumerator NotifyWithDelay(NotifColors colorName, NotifSprites spriteName, string text, float lifespan, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Notify(colorName, spriteName, text, lifespan);
    }
    public void Notify(NotifColors colorName, NotifSprites spriteName, string text, float lifespan)
    {
        //spawneo la notificacion
        GameObject newNotif = Instantiate(messageStandarPrefab, transform.position, Quaternion.identity, canvas.transform);
        NotifPrefabMain notifScript = newNotif.GetComponent<NotifPrefabMain>();

        //inserto la nueva notif como la primera
        activeNotifications.Insert(0, newNotif);

        //le meto el lifespan
        notifScript.lifespan = lifespan;

        // sprite
        if (spriteDic.ContainsKey(spriteName))
            notifScript.image.sprite = spriteDic[spriteName];
        else
            notifScript.image.sprite = spriteDic[NotifSprites._default];

        // color
        if (colorDic.ContainsKey(colorName))
            notifScript.background.color = colorDic[colorName];
        else
            notifScript.background.color = colorDic[NotifColors._default];

        // texto
        if (text != null && text != "")
            notifScript.text.text = text;
        else
            notifScript.text.text = "default_text_error_lol";

        //meto la referencia de este script
        notifScript.notificationManager = this;

        CallRearange();
    }

    public void NotifDeath(GameObject notification)
    {
        //paso por la lista de notificaciones activas y las rearrangeo
        if (activeNotifications.Contains(notification))
            activeNotifications.Remove(notification);

        CallRearange();
    }

    void CallRearange()
    {
        for (int i = 0; i < activeNotifications.Count; i++)
        {
            activeNotifications[i].GetComponent<NotifPrefabMain>().Rearange(i);
        }
    }

}
