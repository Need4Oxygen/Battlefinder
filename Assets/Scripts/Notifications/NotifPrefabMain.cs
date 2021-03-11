using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotifPrefabMain : MonoBehaviour
{
    [HideInInspector]
    public NotificationManager notificationManager;
    public CanvasGroup fadeCanvasGroup;
    public Image background;
    public Image image;
    public TMP_Text text;
    public float speed;
    public float lifespan;
    public float fadeDuration;

    RectTransform self;
    int positionInQueue = 0;
    float height;

    public NotifPrefabMain(Image _image, TMP_Text _text)
    {
        image = _image;
        text = _text;
    }

    public void Rearange(int i)
    {
        positionInQueue = i;
    }

    public void Start()
    {
        self = GetComponent<RectTransform>();
        height = self.sizeDelta.y;

        self.anchoredPosition = new Vector2(0, height);
        self.localPosition = new Vector3(self.localPosition.x, self.localPosition.y, 0);//pongo la z a cero

        StartCoroutine(PanelFader.RescaleAndFadePanel(fadeCanvasGroup.transform, fadeCanvasGroup, 1f, 0f, fadeDuration, null, Destroy));
    }

    private void Update()
    {
        Vector2 targetPos = new Vector2(0, -positionInQueue * height);
        Vector2 distance = targetPos - self.anchoredPosition;

        Vector2 newPos = self.anchoredPosition + distance * speed * Time.unscaledDeltaTime;
        self.anchoredPosition = newPos;
    }

    private void Destroy()
    {
        if (notificationManager != null)
            notificationManager.NotifDeath(gameObject);

        Destroy(gameObject);
    }
}